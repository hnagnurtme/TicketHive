using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using TicketHive.Api.Common;
using TicketHive.Api.Contracts.Authentication;
using TicketHive.Application.Authentication;
using Swashbuckle.AspNetCore.Annotations;


namespace TicketHive.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator , IMapper mapper) : ControllerBase
{
    
    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "Register account",
        Description = "Create a new user account with the provided registration information."
    )]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), StatusCodes.Status201Created)]

    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = mapper.Map<RegisterCommand>(request);
        var result = await mediator.Send(command);

        var response = mapper.Map<AuthenticationResponse>(result);
        return CREATE.HandleResult<AuthenticationResponse>(response, "User registered successfully");
    }


    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Login",
        Description = "Authenticate user and return a JWT token."
    )]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), StatusCodes.Status200OK)]

    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var query = mapper.Map<LoginQuery>(request);
        var result = await mediator.Send(query);

        var response = mapper.Map<AuthenticationResponse>(result.Value);
        return OK.HandleResult<AuthenticationResponse>(response, "User logged in successfully");
    }
}