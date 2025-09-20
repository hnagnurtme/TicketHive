namespace TicketHive.Application.Users.Result;

public record UpdatedUserProfileResult
(
    string UserId,
    string Email,
    string FullName,
    string PhoneNumber
);