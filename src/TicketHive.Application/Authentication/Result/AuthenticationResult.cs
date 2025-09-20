namespace TicketHive.Application.Authentication;

public record AuthenticationResult(string? AccessToken , RefreshTokenDTO? RefreshToken ,UserDTO User);

public record UserDTO
(
    Guid Id,
    string Email,
    string FullName,
    string PhoneNumber,
    bool EmailVerified,
    DateTime CreatedAt ,
    DateTime UpdatedAt
);

public record RefreshTokenDTO
(
    string RefreshToken,
    DateTime RefreshTokenExpiresAt
);
