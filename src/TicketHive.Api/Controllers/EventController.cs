using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TicketHive.Api.Contracts.Events;
using TicketHive.Application.Events.Command;
using TicketHive.Application.Events;
using ErrorOr;
using TicketHive.Api.Common.Helpers;
using TicketHive.Api.Common;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace TicketHive.Api.Controllers;

[ApiController]
[Authorize(Roles = "ADMIN, ORGANIZER")]
[Route("api/event")]
public class EventController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost()]
    [SwaggerOperation(
        Summary = "Add new event",
        Description = "Create a new event with the provided event information."
    )]
    [ProducesResponseType(typeof(ApiResponse<AddEventResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddEvent([FromBody] AddEventRequest addEventRequest)
    {
        var command = mapper.Map<AddEventCommand>(addEventRequest);
        ErrorOr<AddEventResult> result = await mediator.Send(command);
        var response = result.MapTo<AddEventResult, AddEventResponse>(mapper);
        return OK.HandleResult(response, "Event created successfully");
    }


    [HttpPatch("publish")]
    [SwaggerOperation(
        Summary = "Publish an event",
        Description = "Publish an event to make it publicly available."
    )]
    [ProducesResponseType(typeof(ApiResponse<EventResponse>), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PublishEvent([FromBody] PublishEventRequest publishEventRequest)
    {
        var command = mapper.Map<PushlishEventCommand>(publishEventRequest);
        ErrorOr<PublishEventResult> result = await mediator.Send(command);
        var response = result.MapTo<PublishEventResult, EventResponse>(mapper);
        return OK.HandleResult(response, "Event published successfully");
    }
}