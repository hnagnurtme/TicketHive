using TicketHive.Domain.Entities;
namespace TicketHive.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}