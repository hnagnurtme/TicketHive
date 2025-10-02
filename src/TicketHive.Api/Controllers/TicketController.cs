using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using TicketHive.Api.Common;
using TicketHive.Api.Common.Helpers;
using TicketHive.Api.Contracts.Tickets;
using TicketHive.Application.Common;
using TicketHive.Application.Tickets;

namespace TicketHive.Api.Controllers;


[ApiController]
[Authorize(Roles = "ADMIN, ORGANIZER")]
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

    [HttpGet()]
    [SwaggerOperation(
        Summary = "Get tickets with pagination",
        Description = "Retrieve tickets with pagination, filtering, and sorting options."
    )]
    [ProducesResponseType(typeof(ApiResponse<GetTicketsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTickets([FromQuery] GetTicketsRequest request)
    {
        var query = mapper.Map<GetTicketsQuery>(request);
        var result = await mediator.Send(query);
        var response = result.MapTo<PagedResult<TicketResult>, GetTicketsResponse>(mapper);
        return OK.HandleResult(response, "Tickets retrieved successfully");
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get ticket by ID",
        Description = "Retrieve detailed information about a specific ticket."
    )]
    [ProducesResponseType(typeof(ApiResponse<GetTicketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTicket([FromRoute] Guid id)
    {
        var query = new GetTicketByIdQuery(id);
        var result = await mediator.Send(query);
        var response = result.MapTo<TicketDetailResult, GetTicketResponse>(mapper);
        return OK.HandleResult(response, "Ticket retrieved successfully");
    }

    [HttpGet("events/{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Get tickets by event ID",
        Description = "Retrieve all tickets for a specific event."
    )]
    [ProducesResponseType(typeof(ApiResponse<GetTicketsByEventResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTicketsByEvent([FromRoute] Guid eventId)
    {
        var query = new GetTicketsByEventIdQuery(eventId);
        var result = await mediator.Send(query);
        var response = result.MapTo<List<TicketResult>, GetTicketsByEventResponse>(mapper);
        return OK.HandleResult(response, "Tickets retrieved successfully");
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Update ticket",
        Description = "Update an existing ticket with new information."
    )]
    [ProducesResponseType(typeof(ApiResponse<UpdateTicketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateTicket([FromRoute] Guid id, [FromBody] UpdateTicketRequest updateTicketRequest)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
        var command = mapper.Map<UpdateTicketCommand>(updateTicketRequest);
        command = command with { TicketId = id};
        
        var result = await mediator.Send(command);
        var response = result.MapTo<TicketDetailResult, UpdateTicketResponse>(mapper);
        return OK.HandleResult(response, "Ticket updated successfully");
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Delete ticket",
        Description = "Permanently delete a ticket. Only allowed if no sales exist."
    )]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteTicket([FromRoute] Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
        var command = new DeleteTicketCommand(id);
        var result = await mediator.Send(command);
        var response = result.MapTo<bool, bool>(mapper);
        return OK.HandleResult(response, "Ticket deleted successfully");
    }

    [HttpPatch("{id:guid}/deactivate")]
    [SwaggerOperation(
        Summary = "Deactivate ticket",
        Description = "Deactivate a ticket to stop sales while preserving existing data."
    )]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeactivateTicket([FromRoute] Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
        var command = new DeactivateTicketCommand(id);
        var result = await mediator.Send(command);
        var response = result.MapTo<bool, bool>(mapper);
        return OK.HandleResult(response, "Ticket deactivated successfully");
    }
}