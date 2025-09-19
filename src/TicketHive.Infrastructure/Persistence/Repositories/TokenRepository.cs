using Microsoft.EntityFrameworkCore;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;
using TicketHive.Infrastructure.Persistence;

namespace TicketHive.Infrastructure.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly AppDbContext _dbContext;

    public TokenRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.RefreshTokens.FindAsync(new object?[] { id }, cancellationToken);

    public async Task<RefreshToken?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(rt => rt.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    public async Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        await _dbContext.RefreshTokens.AddAsync(token, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        _dbContext.RefreshTokens.Update(token);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ExecuteUpdateAsync(updates => updates.SetProperty(rt => rt.RevokedAt, DateTime.UtcNow),
                                cancellationToken);
    }
}
