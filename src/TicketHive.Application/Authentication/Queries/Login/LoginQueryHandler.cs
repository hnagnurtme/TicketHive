namespace TicketHive.Application.Authentication;

using MediatR;
using TicketHive.Application.Common.Interfaces;
using System.Security.Claims;
using ErrorOr;
using TicketHive.Domain.Exceptions;
using TicketHive.Application.Common.Interfaces.Repositories;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IHashService _hashService;
    private readonly IMediator _mediator;

    public LoginQueryHandler(IUnitOfWork unitOfWork, IJwtService jwtService, IHashService hashService, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _hashService = hashService;
        _mediator = mediator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new UnAuthorizationException("Invalid credentials.");
        }
        // Check if email is verified
        if (!user.EmailVerified)
        {
            throw new EmailNotVerifyException("Email is not verified. Please verify your email before logging in.");
        }

        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnAuthorizationException("Invalid credentials.");
        }
        user.UpdateLogin(DateTime.UtcNow);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var accessToken = _jwtService.GenerateToken(claims);

        var refreshTokenCommand = new GenerateRefreshTokenCommand(
            user.Id,
            request.IpAddress,
            request.UserAgent,
            request.DeviceFingerprint
        );

        var refreshTokenResult = await _mediator.Send(refreshTokenCommand, cancellationToken);

        var userDto = new UserDTO(
            user.Id,
            user.Email,
            user.FullName ?? string.Empty,
            user.PhoneNumber ?? string.Empty,
            user.EmailVerified,
            user.CreatedAt,
            user.UpdatedAt
        );

        var tokenDto = new RefreshTokenDTO(
            refreshTokenResult.Value.Token,
            refreshTokenResult.Value.ExpiresAt
        );

        return new AuthenticationResult(accessToken, tokenDto, userDto);
    }

    private bool VerifyPassword(string password, string passwordHash)
        => _hashService.Verify(password, passwordHash);
}

