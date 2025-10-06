using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TicketHive.Api.Common;
using TicketHive.Api.Common.Helpers;
using TicketHive.Api.Contracts.Orders;
using TicketHive.Application.Orders.Commands.CreateOrder;
using TicketHive.Application.Orders.Result;
using System.Security.Claims;

namespace TicketHive.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost()]
    [Authorize(Roles = "USER")]
    [SwaggerOperation(
        Summary = "Create new order",
        Description = "Create a new order with the provided order information."
    )]
    [ProducesResponseType(typeof(ApiResponse<CreateOrderResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest createOrderRequest)
    {

        var command = mapper.Map<CreateOrderCommand>(createOrderRequest);
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); 
        if (Guid.TryParse(userIdString, out var userGuid))
        {
            command = command with { UserId = userGuid };
        }
        else
        {
            return Unauthorized("Invalid user ID");
        }
        

        var result = await mediator.Send(command);
        var response = result.MapTo<OrderResult, CreateOrderResponse>(mapper);
        return OK.HandleResult(response, "Order created successfully");
    }
}