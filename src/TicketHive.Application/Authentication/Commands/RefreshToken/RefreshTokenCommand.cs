using ErrorOr;
using MediatR;

namespace TicketHive.Application.Authentication;

public record GenerateRefreshTokenCommand(Guid UserId, string IpAddress, string UserAgent, string? DeviceFingerprint) 
    : IRequest<ErrorOr<RefreshTokenResult>>;
