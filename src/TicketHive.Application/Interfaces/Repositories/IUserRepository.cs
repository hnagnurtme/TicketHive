using TicketHive.Domain.Entities;
using System.Threading.Tasks;
namespace TicketHive.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task CreateUserAsync(User user);

    Task<User?> GetUserByEmailAsync(string email);

    Task<bool> ExistsByEmailAsync(string email);

    Task UpdateUserAsync(User user);

}