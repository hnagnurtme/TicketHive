using System.Security.Claims;
namespace TicketHive.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims, TimeSpan? lifetime = null);

    ClaimsPrincipal? ValidateToken(string token);
}