using System.Security.Claims;
namespace TicketHive.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims);

    ClaimsPrincipal? ValidateToken(string token);
}