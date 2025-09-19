using MediatR;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Common.Interfaces;
using ErrorOr;
using System.Security.Claims;

namespace TicketHive.Application.Authentication.Commands.RefreshToken;

public class ValidateRefreshTokenCommandHandler
    : IRequestHandler<ValidateRefreshTokenCommand, ErrorOr<AuthenticationResult>>
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IHashService _hashService;
    private readonly IMediator _mediator;

    public ValidateRefreshTokenCommandHandler(
        ITokenRepository tokenRepository,
        IJwtService jwtService,
        IHashService hashService,
        IMediator mediator)
    {
        _tokenRepository = tokenRepository;
        _jwtService = jwtService;
        _hashService = hashService;
        _mediator = mediator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        ValidateRefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        System.Console.WriteLine($"Validating refresh token for UserId: {request.UserId}, IpAddress: {request.IpAddress}, UserAgent: {request.UserAgent}, DeviceFingerprint: {request.DeviceFingerprint}");
        // 1. Lấy tất cả refresh token active của user
        var tokens = await _tokenRepository.GetActiveTokensByUserIdAsync(request.UserId, cancellationToken);

        // 2. So sánh refresh token client gửi với hash trong DB
        var token = tokens.FirstOrDefault(t => 
            _hashService.Verify(request.RefreshToken, t.TokenHash));

        if (token == null || !token.IsActive)
        {
            return Error.Unauthorized(description: "Invalid or expired refresh token.");
        }

        // 3. Optional: check userAgent / deviceFingerprint
        if (!string.IsNullOrEmpty(token.UserAgent) &&
            !string.Equals(token.UserAgent, request.UserAgent, StringComparison.OrdinalIgnoreCase))
        {
            return Error.Unauthorized(description: "Refresh token does not match the device (UserAgent mismatch).");
        }

        if (!string.IsNullOrEmpty(token.DeviceFingerprint) &&
            !string.Equals(token.DeviceFingerprint, request.DeviceFingerprint, StringComparison.OrdinalIgnoreCase))
        {
            return Error.Unauthorized(description: "Refresh token does not match the device (Fingerprint mismatch).");
        }

        // IP address thường hay thay đổi → chỉ log warning
        if (!string.IsNullOrEmpty(token.IpAddress) &&
            !string.Equals(token.IpAddress, request.IpAddress, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"⚠️ Warning: IP mismatch for refresh token. Expected {token.IpAddress}, got {request.IpAddress}");
        }

        // 4. Đánh dấu token cũ đã dùng
        token.MarkAsUsed();
        await _tokenRepository.UpdateAsync(token, cancellationToken);

        // 5. Sinh access token mới
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, token.User.Id.ToString()),
            new Claim(ClaimTypes.Email, token.User.Email),
            new Claim(ClaimTypes.Name, token.User.FullName ?? string.Empty),
            new Claim(ClaimTypes.MobilePhone, token.User.PhoneNumber ?? string.Empty),
            new Claim(ClaimTypes.Role, token.User.Role.ToString())
        };
        var accessToken = _jwtService.GenerateToken(claims);

        // 6. Sinh refresh token mới
        var refreshCommand = new GenerateRefreshTokenCommand(
            token.UserId,
            request.IpAddress,
            request.UserAgent,
            request.DeviceFingerprint
        );
        var newRefreshResult = await _mediator.Send(refreshCommand, cancellationToken);

        // 7. Build DTO
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
