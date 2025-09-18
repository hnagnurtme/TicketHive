using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace TicketHive.Api.Controllers.User;

[ApiController]
[Route("api/users")]

public class UserController(IMediator mediator) : ControllerBase
{

    /// <summary>
    /// Lấy thông tin hồ sơ người dùng hiện tại.
    /// </summary>
    /// <returns>Thông tin hồ sơ người dùng.</returns>
    [HttpGet("profile")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Lấy hồ sơ người dùng",
        Description = "Trả về thông tin hồ sơ của người dùng đã xác thực."
    )]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Thay string bằng model thực tế nếu có
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProfile()
    {

        return Ok(" Test");
    }
}
