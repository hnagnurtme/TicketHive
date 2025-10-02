namespace TicketHive.Application.Events;

public record EventResult(
    Guid EventId,
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
    string Status
);