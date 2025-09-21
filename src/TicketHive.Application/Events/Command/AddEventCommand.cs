using MediatR;
using ErrorOr;

namespace TicketHive.Application.Events.Command;

public record AddEventCommand(
    string Name,
    string Slug,
    string? Description,
    string Location,
    DateTime StartTime,
    DateTime EndTime,
    int? VenueCapacity,
    DateTime? SaleStartTime,
    DateTime? SaleEndTime,
    string? ImageUrl,
    bool IsFeatured,
    Guid OrganizerId
) : IRequest<ErrorOr<AddEventResult>>;
