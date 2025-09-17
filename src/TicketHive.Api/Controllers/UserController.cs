using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace TicketHive.Api.Controllers.User;

[ApiController]
[Route("api/users")]

public class UserController(IMediator mediator) : ControllerBase
{

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {

        return Ok(" Test");
    }
}
