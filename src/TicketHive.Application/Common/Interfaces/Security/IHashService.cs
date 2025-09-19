namespace TicketHive.Application.Common.Interfaces;
public interface IHashService
{
    string Hash(string input);
    bool Verify(string hash, string input);
}