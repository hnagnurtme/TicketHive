using ErrorOr;
using MediatR;

namespace TicketHive.Application.Events;

public record UpdateEventCommand(
    Guid EventId,
    string Name,
    string? Description,
    string Location,
    DateTime StartTime,
    DateTime EndTime,
    int? VenueCapacity,
    DateTime? SaleStartTime,
    DateTime? SaleEndTime,
    string? ImageUrl,
    bool IsFeatured
) : IRequest<ErrorOr<EventDetailResult>>;