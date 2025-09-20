namespace TicketHive.Application.Users.Result;

public record UserProfileResult
(
    string Email,
    string FullName,
    string PhoneNumber
);