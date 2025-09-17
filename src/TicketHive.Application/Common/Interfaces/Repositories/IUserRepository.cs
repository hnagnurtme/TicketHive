using TicketHive.Domain.Entities;
namespace TicketHive.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task CreateUserAsync(User user);

    Task<User?> GetUserByEmailAsync(string email);

    Task<bool> ExistsByEmailAsync(string email);

    Task UpdateUserAsync(User user);

}