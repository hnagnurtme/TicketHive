using Microsoft.Extensions.Configuration;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Domain.Entities;

namespace TicketHive.Infrastructure.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly IHashService _hashService;
    private readonly IConfiguration _configuration;

    public RefreshTokenGenerator(IHashService hashService, IConfiguration configuration)
    {
        _hashService = hashService;
        _configuration = configuration;
    }

    public RefreshToken Generate(
        Guid userId,
        string? ipAddress,
        string? userAgent,
        string? deviceFingerprint,
        out string plainToken)
    {
        plainToken = Guid.NewGuid().ToString("N");

        var tokenHash = _hashService.Hash(plainToken);

        var lifetimeDays = _configuration.GetValue<int?>("Auth:RefreshTokenLifetimeDays") ?? 7;
        var expiresAt = DateTime.UtcNow.AddDays(lifetimeDays);

        return new RefreshToken(
            userId,
            tokenHash,
            expiresAt,
            ipAddress,
            userAgent,
            deviceFingerprint
        );
    }
}
