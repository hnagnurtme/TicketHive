using ErrorOr;
using MediatR;

namespace TicketHive.Application.Tickets;

public record UpdateTicketCommand(
    Guid TicketId,
    string Type,
    string Name,
    decimal Price,
    int TotalQuantity,
    int MinPurchase,
    int MaxPurchase,
    string? Description,
    decimal? OriginalPrice,
    DateTime? SaleStartTime,
    DateTime? SaleEndTime,
    bool IsActive,
    int SortOrder
) : IRequest<ErrorOr<TicketDetailResult>>;