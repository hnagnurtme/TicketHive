namespace TicketHive.Application.Events;

public record EventDetailResult(
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
    Guid OrganizerId,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid CreatedBy,
    Guid? UpdatedBy
);