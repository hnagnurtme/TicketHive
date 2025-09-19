using TicketHive.Domain.Entities;

namespace TicketHive.Application.Common.Interfaces.Repositories;

public interface ITokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<RefreshToken?> GetLatestActiveTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<RefreshToken?> GetValidTokenAsync(Guid userId,string userAgent, string deviceFingerprint, CancellationToken cancellationToken = default);
}
