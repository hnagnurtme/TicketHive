namespace TicketHive.Api.Contracts.Authentication;

public record VerifyEmailParam(string Email, string Token);