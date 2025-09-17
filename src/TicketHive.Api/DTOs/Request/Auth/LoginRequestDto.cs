namespace TicketHive.Api.DTOs.Request.Auth;

public class LoginRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}