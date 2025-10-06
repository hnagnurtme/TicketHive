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
using TicketHive.Application.Common;

namespace TicketHive.Api.Controllers;

[ApiController]
[Authorize(Roles = "ADMIN, ORGANIZER")]
[Route("api/events")]
public class EventController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost()]
    [SwaggerOperation(
        Summary = "Add new event",
        Description = "Create a new event with the provided event information."
    )]
    [ProducesResponseType(typeof(ApiResponse<AddEventResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddEvent([FromBody] AddEventRequest addEventRequest)
    {
        var command = mapper.Map<AddEventCommand>(addEventRequest);
        ErrorOr<AddEventResult> result = await mediator.Send(command);
        var response = result.MapTo<AddEventResult, AddEventResponse>(mapper);
        return OK.HandleResult(response, "Event created successfully");
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get event by ID",
        Description = "Retrieve detailed information about a specific event."
    )]
    [ProducesResponseType(typeof(ApiResponse<GetEventResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEvent([FromRoute] Guid id)
    {
        var query = new GetEventByIdQuery(id);
        var result = await mediator.Send(query);
        var response = result.MapTo<EventDetailResult, GetEventResponse>(mapper);
        return OK.HandleResult(response, "Event retrieved successfully");
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all events",
        Description = "Retrieve all events."
    )]
    [ProducesResponseType(typeof(ApiResponse<GetEventsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEvents()
    {
        var query = new GetAllEventsQuery();
        var result = await mediator.Send(query);
        var response = result.MapTo<List<EventResult>, GetEventsResponse>(mapper);
        return OK.HandleResult(response, "Events retrieved successfully");
    }

    [HttpGet("paged")]
    [SwaggerOperation(
        Summary = "Get events with pagination",
        Description = "Retrieve events with pagination and filtering options."
    )]
    [ProducesResponseType(typeof(ApiResponse<GetPagedEventsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagedEvents([FromQuery] GetPagedEventsRequest request)
    {
        var query = mapper.Map<GetPagedEventsQuery>(request);
        var result = await mediator.Send(query);
        var response = result.MapTo<PagedResult<EventResult>, GetPagedEventsResponse>(mapper);
        return OK.HandleResult(response, "Paged events retrieved successfully");
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Update event",
        Description = "Update an existing event with new information."
    )]
    [ProducesResponseType(typeof(ApiResponse<UpdateEventResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEvent([FromRoute] Guid id, [FromBody] UpdateEventRequest updateEventRequest)
    {
        var command = mapper.Map<UpdateEventCommand>(updateEventRequest);
        command = command with { EventId = id };
        
        var result = await mediator.Send(command);
        var response = result.MapTo<EventDetailResult, UpdateEventResponse>(mapper);
        return OK.HandleResult(response, "Event updated successfully");
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Delete event",
        Description = "Permanently delete an event. Only allowed if no tickets exist."
    )]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteEvent([FromRoute] Guid id)
    {
        var command = new DeleteEventCommand(id);
        var result = await mediator.Send(command);
        var response = result.MapTo<bool, bool>(mapper);
        return OK.HandleResult(response, "Event deleted successfully");
    }

    [HttpPatch("publish")]
    [SwaggerOperation(
        Summary = "Publish an event",
        Description = "Publish an event to make it publicly available."
    )]
    [ProducesResponseType(typeof(ApiResponse<EventResponse>), StatusCodes.Status200OK)] 
    public async Task<IActionResult> PublishEvent([FromBody] PublishEventRequest publishEventRequest)
    {
        var command = mapper.Map<PushlishEventCommand>(publishEventRequest);
        ErrorOr<PublishEventResult> result = await mediator.Send(command);
        var response = result.MapTo<PublishEventResult, EventResponse>(mapper);
        return OK.HandleResult(response, "Event published successfully");
    }
}