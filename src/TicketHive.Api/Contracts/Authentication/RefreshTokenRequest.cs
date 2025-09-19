namespace TicketHive.Api.Contracts.Authentication;

public class RefreshTokenRequest
{
    public required Guid UserId { get; set; }
    public required string Token { get; set; }
    public required string IpAddress { get; set; }
    public required string UserAgent { get; set; }
    public string? DeviceFingerprint { get; set; }
}