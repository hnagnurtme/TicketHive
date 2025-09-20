using AutoMapper;
using MediatR;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TicketHive.Application.Users.Query;
using TicketHive.Api.Common;
using TicketHive.Api.Common.Helpers;
using TicketHive.Api.Contracts.Users;

namespace TicketHive.Api.Controllers.User;

[ApiController]
[Route("api/users")]

public class UserController(IMediator mediator, IMapper mapper) : ControllerBase
{

    [HttpGet("profile/{userId}")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Get User Profile",
        Description = "Get the profile information of the authenticated user."
    )]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProfile([FromRoute] string userId)
    {
        var query = new GetUserProfileQuery(userId);
        ErrorOr<UserProfileResult> result = await mediator.Send(query);
        var response = result.MapTo<UserProfileResult, UserProfileResponse>(mapper);
        return OK.HandleResult(response, "User profile retrieved successfully");
    }
    
    
}
