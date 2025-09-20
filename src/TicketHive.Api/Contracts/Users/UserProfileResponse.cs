namespace TicketHive.Api.Contracts.Users;

public record UserProfileResponse
(
    string Email,
    string FullName,
    string PhoneNumber
);