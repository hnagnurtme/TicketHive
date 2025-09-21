namespace TicketHive.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string? Email { get; }
    string? FullName { get; }
    string? PhoneNumber { get; }
    string? Role { get; }
}