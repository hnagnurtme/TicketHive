using TicketHive.Domain.Entities;

namespace TicketHive.Application.Common.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);


    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetWithTokensAsync(Guid userId, CancellationToken cancellationToken = default);
    
}
