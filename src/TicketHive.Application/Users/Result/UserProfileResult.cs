namespace TicketHive.Application.Users.Query;

public record UserProfileResult
(
    string Email,
    string FullName,
    string PhoneNumber
);