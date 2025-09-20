using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using TicketHive.Api.Common;
using TicketHive.Api.Contracts.Authentication;
using TicketHive.Application.Authentication;
using Swashbuckle.AspNetCore.Annotations;
using TicketHive.Api.Common.Helpers;
using TicketHive.Application.Authentication.Commands.RefreshToken;


namespace TicketHive.Api.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController(IMediator mediator, IMapper mapper) : ControllerBase
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
        ErrorOr<AuthenticationResult> result = await mediator.Send(command);
        var response = result.MapTo<AuthenticationResult, AuthenticationResponse>(mapper);
        return OK.HandleResult(response, "Login success");
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
        var response = result.MapTo<AuthenticationResult, AuthenticationResponse>(mapper);
        return OK.HandleResult(response, "Login success");
    }
    

    [HttpPost("refresh-token")]
    [SwaggerOperation(
        Summary = "Generate refresh token",
        Description = "Generate a new refresh token for the authenticated user."
    )]
    [ProducesResponseType(typeof(ApiResponse<RefreshTokenResponse>), StatusCodes.Status200OK)]

    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var command = mapper.Map<ValidateRefreshTokenCommand>(request);
        var result = await mediator.Send(command);

        var response = result.MapTo<AuthenticationResult, AuthenticationResponse>(mapper);
        return OK.HandleResult(response, "Refresh token success");
    }
}
