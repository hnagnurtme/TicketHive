using MediatR;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Common.Interfaces;
using ErrorOr;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace TicketHive.Application.Authentication.Commands.RefreshToken;

public class ValidateRefreshTokenCommandHandler
    : IRequestHandler<ValidateRefreshTokenCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IHashService _hashService;
    private readonly IMediator _mediator;
    private readonly ILogger<ValidateRefreshTokenCommandHandler> _logger;

    public ValidateRefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IHashService hashService,
        ILogger<ValidateRefreshTokenCommandHandler> logger,
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _hashService = hashService;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        ValidateRefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var tokenRepo = _unitOfWork.Tokens;

        var token = await tokenRepo.GetValidTokenAsync(
            request.UserId,
            request.UserAgent,
            request.DeviceFingerprint,
            cancellationToken);
    
        if (token == null || !_hashService.Verify(request.RefreshToken, token.TokenHash) || !token.IsActive)
        {
            return Error.Unauthorized(description: "Invalid or expired refresh token.");
        }
        if (!string.IsNullOrEmpty(token.IpAddress) &&
            !string.Equals(token.IpAddress, request.IpAddress, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("IP mismatch for refresh token. Expected {Expected}, got {Actual}", 
                token.IpAddress, request.IpAddress);
        }

        token.MarkAsUsed();
        token.Replace(token.Id);
        
        tokenRepo.Update(token);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, token.User.Id.ToString()),
            new Claim(ClaimTypes.Email, token.User.Email),
            new Claim(ClaimTypes.Name, token.User.FullName ?? string.Empty),
            new Claim(ClaimTypes.MobilePhone, token.User.PhoneNumber ?? string.Empty),
            new Claim(ClaimTypes.Role, token.User.Role.ToString())
        };
        var accessToken = _jwtService.GenerateToken(claims);

        var refreshCommand = new GenerateRefreshTokenCommand(
            token.UserId,
            request.IpAddress,
            request.UserAgent,
            request.DeviceFingerprint
        );
        var newRefreshResult = await _mediator.Send(refreshCommand, cancellationToken);

        var userDto = new UserDTO(
            token.User.Id,
            token.User.Email,
            token.User.FullName ?? string.Empty,
            token.User.PhoneNumber ?? string.Empty,
            token.User.CreatedAt,
            token.User.UpdatedAt
        );

        var refreshDto = new RefreshTokenDTO(
            newRefreshResult.Value.Token,
            newRefreshResult.Value.ExpiresAt
        );

        return new AuthenticationResult(accessToken, refreshDto, userDto);
    }
}
