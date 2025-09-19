using Microsoft.EntityFrameworkCore;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;

namespace TicketHive.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{

    public UserRepository(AppDbContext dbContext) : base(dbContext) { }
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetWithTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users
            .AnyAsync(u => u.Email == email, cancellationToken);
    }
}
