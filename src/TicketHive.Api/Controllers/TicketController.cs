using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TicketHive.Api.Common;
using TicketHive.Api.Common.Helpers;
using TicketHive.Api.Contracts.Tickets;
using TicketHive.Application.Tickets;

namespace TicketHive.Api.Controllers;


[ApiController]
// [Authorize(Roles = "ADMIN, ORGANIZER")]
[Route("api/tickets")]
public class TicketController( IMediator mediator, IMapper mapper) : ControllerBase 
{
    [HttpPost()]
    [SwaggerOperation(
        Summary = "Add new ticket",
        Description = "Create a new ticket with the provided ticket information."
    )]
    [ProducesResponseType(typeof(ApiResponse<AddTicketResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddTicket([FromBody] AddTicketRequest addTicketRequest)
    {
        var command = mapper.Map<CreateTicketCommand>(addTicketRequest);
        var result = await mediator.Send(command);
        var response = result.MapTo<TicketResult, AddTicketResponse>(mapper);
        return OK.HandleResult(response, "Ticket created successfully");
    }
}