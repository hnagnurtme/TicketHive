namespace TicketHive.Application.Authentication;

public record AuthenticationResult(string Token, UserDTO User);

public record UserDTO
(
    Guid Id,
    string Email,
    string FullName,
    string PhoneNumber,
    DateTime CreatedAt ,
    DateTime UpdatedAt
);

