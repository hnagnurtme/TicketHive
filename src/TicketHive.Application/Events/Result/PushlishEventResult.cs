using TicketHive.Domain.Entities;

namespace TicketHive.Application.Events;

public record PublishEventResult(
    Guid Id,
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
    Guid OrganizerId,
    EventStatus Status,
    DateTime CreatedAt
);
