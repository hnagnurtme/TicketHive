namespace TicketHive.Api.Contracts.Authentication;

public class AuthenticationResponse
{
    public required string RefreshToken { get; set; }
    public required string AccessToken { get; set; }
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
    
    public required bool EmailVerified { get; set; }
}