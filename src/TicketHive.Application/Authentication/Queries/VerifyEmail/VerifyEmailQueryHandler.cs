namespace TicketHive.Application.Authentication;
using MediatR;
using ErrorOr;
using TicketHive.Application.Common.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Exceptions;
using TicketHive.Application.Common.Interfaces;
using System.Security.Claims;

public class VerifyEmailQueryHandler : IRequestHandler<VerifyEmailQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<VerifyEmailQueryHandler> _logger;

    private readonly IJwtService _jwtService;

    public VerifyEmailQueryHandler(IUnitOfWork unitOfWork, ILogger<VerifyEmailQueryHandler> logger, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _jwtService = jwtService;
    }
    public async Task<ErrorOr<AuthenticationResult>> Handle(VerifyEmailQuery request, CancellationToken cancellationToken)
    {

        // Check email exists
        var user = await _unitOfWork.User.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Email {Email} not found during verification.", request.Email);
            throw new NotFoundException("Email not found.");
        }

        // Check token validity
        var principal = _jwtService.ValidateToken(request.Token);

        if (principal == null)
        {
            _logger.LogWarning("Invalid token provided for email {Email}.", request.Email);
            throw new UnAuthorizationException("Invalid token.");
        }

        var emailClaim = principal.FindFirst(ClaimTypes.Email);
        if (emailClaim == null || emailClaim.Value != request.Email)
        {
            _logger.LogWarning("Token email claim does not match the provided email {Email}.", request.Email);
            throw new UnAuthorizationException("Token email claim does not match.");
        }
        // Mark email as verified
        user.MarkEmailAsVerified();

        _unitOfWork.User.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Email {Email} successfully verified.", request.Email);
        // Return user info
        var userDto = new UserDTO(
            user.Id,
            user.Email,
            user.FullName ?? string.Empty,
            user.PhoneNumber ?? string.Empty,
            user.EmailVerified,
            user.CreatedAt,
            user.UpdatedAt
        );
        return new AuthenticationResult(null,null, userDto);
    }
}