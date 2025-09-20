using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace TicketHive.Api.Contracts.Authentication;

public class LoginRequest
{
    [SwaggerSchema(Description = "User's email address")]
    public required string Email { get; set; }
    [SwaggerSchema(Description = "User's password")]
    public required string Password { get; set; }
    [SwaggerSchema(Description = "The IP address of the client making the request")]
    public required string IpAddress { get; set; }
    [SwaggerSchema(Description = "The User-Agent string of the client making the request")]
    public required string UserAgent { get; set; }
    [SwaggerSchema(Description = "An optional device fingerprint to uniquely identify the device")]
    public string? DeviceFingerprint { get; set; }
}