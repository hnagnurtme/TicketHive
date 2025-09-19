namespace TicketHive.Application.Common.Interfaces;

using TicketHive.Domain.Entities;

public interface IRefreshTokenGenerator
{
    RefreshToken Generate(
        Guid userId,
        string? ipAddress,
        string? userAgent,
        string? deviceFingerprint
    , out string plainToken);
}