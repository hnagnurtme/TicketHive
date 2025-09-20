namespace TicketHive.Api.Contracts.Users;

public record UpdateUserProfileRequest
(
    string? FullName,
    string? PhoneNumber
);

