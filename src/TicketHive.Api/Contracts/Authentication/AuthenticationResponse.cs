namespace TicketHive.Api.Contracts.Authentication;
public class AuthenticationResponse
{
    public required string Token { get; set; }
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
}