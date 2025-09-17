using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketHive.Api.DTOs.Request.Auth;
using TicketHive.Application.Commands.Auth;

namespace TicketHive.Api.Controllers.Auth;

public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        return Ok(await mediator.Send(new RegisterCommand(request.Email, request.Password, request.FullName, request.PhoneNumber)));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        return Ok(await mediator.Send(new LoginCommand(request.Email, request.Password)));
    }
}