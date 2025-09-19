using MediatR;
using ErrorOr;

namespace TicketHive.Application.Authentication.Commands.RefreshToken;

public record ValidateRefreshTokenCommand( Guid UserId , string RefreshToken , string IpAddress , string UserAgent , string DeviceFingerprint)
    : IRequest<ErrorOr<AuthenticationResult>>;
