using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace TicketHive.Api.Contracts.Authentication;

public class LoginRequest
{
    [SwaggerSchema(Description = "User's email address")]
    public required string Email { get; set; }
    [SwaggerSchema(Description = "User's password")]
    public required string Password { get; set; }
}