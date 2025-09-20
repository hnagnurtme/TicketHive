using Microsoft.EntityFrameworkCore;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;

namespace TicketHive.Infrastructure.Persistence.Repositories;

public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
{

    public TokenRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<RefreshToken?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(rt => rt.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.Revoke();
        }

        _dbContext.RefreshTokens.UpdateRange(tokens);
    }

    public Task<RefreshToken?> GetLatestActiveTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow && rt.Used == false)
            .OrderByDescending(rt => rt.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<RefreshToken?> GetValidTokenAsync(Guid userId, string userAgent, string deviceFingerprint, CancellationToken cancellationToken = default)
    {
        return _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow && rt.Used == false && rt.UserAgent == userAgent && rt.DeviceFingerprint == deviceFingerprint)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
