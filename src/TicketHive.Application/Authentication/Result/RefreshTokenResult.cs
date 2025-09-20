namespace TicketHive.Application.Authentication;

public record RefreshTokenResult(string Token , DateTime ExpiresAt );