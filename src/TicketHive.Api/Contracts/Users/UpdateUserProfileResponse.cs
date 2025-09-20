namespace TicketHive.Api.Contracts.Users;


public record UpdateUserProfileResponse
(
    string UserId,
    string Email,
    string FullName,
    string PhoneNumber
);