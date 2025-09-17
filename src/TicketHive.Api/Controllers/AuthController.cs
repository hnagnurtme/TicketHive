using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketHive.Api.Common;
using TicketHive.Api.Contracts.Authentication;
using TicketHive.Application.Authentication;


namespace TicketHive.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator , IMapper mapper) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = mapper.Map<RegisterCommand>(request);
        var result = await mediator.Send(command);

        var response = mapper.Map<AuthenticationResponse>(result);
        return CREATE.HandleResult<AuthenticationResponse>(response, "User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var query = mapper.Map<LoginQuery>(request);
        var result = await mediator.Send(query);

        var response = mapper.Map<AuthenticationResponse>(result.Value);
        return OK.HandleResult<AuthenticationResponse>(response, "User logged in successfully");
    }
}