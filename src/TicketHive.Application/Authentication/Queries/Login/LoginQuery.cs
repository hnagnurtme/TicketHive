using MediatR;
using ErrorOr;

namespace TicketHive.Application.Authentication;

public record LoginQuery(
    string Email,
    string Password,
    string IpAddress,
    string UserAgent,
    string DeviceFingerprint
) : IRequest<ErrorOr<AuthenticationResult>>;
