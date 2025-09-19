using TicketHive.Domain.Entities;
namespace TicketHive.Application.Common.Interfaces.Repositories;

public interface ITokenRepository
{
    Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task UpdateAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
