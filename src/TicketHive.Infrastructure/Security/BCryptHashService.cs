using BCrypt.Net;
using TicketHive.Application.Common.Interfaces;

namespace TicketHive.Infrastructure.Security;

public class BCryptHashService : IHashService
{
    private const int WorkFactor = 12;

    public string Hash(string input)
    {
        return BCrypt.Net.BCrypt.HashPassword(input, workFactor: WorkFactor);
    }

    public bool Verify(string input, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(input, hash);
    }
}