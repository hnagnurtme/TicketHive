using ErrorOr;
using MediatR;
using TicketHive.Application.Orders.Result;

namespace TicketHive.Application.Orders.Commands.CreateOrder;
public record CreateOrderItem(Guid TicketId, int Quantity);
public record CreateOrderCommand(
    Guid UserId, 
    List<CreateOrderItem> Items,
    string PaymentProvider,
    string? CouponCode = null
) : IRequest<ErrorOr<OrderResult>>;