using System.ComponentModel.DataAnnotations;
using TicketHive.Domain.Exceptions;
using TicketHive.Domain.Exceptions.Base;

namespace TicketHive.Domain.Entities;

/// <summary>
/// Represents an event in the TicketHive system.
/// </summary>
public class Event
{
    /// <summary>
    /// Event unique identifier.
    /// </summary>
    public Guid Id { get; private set; }

    [MaxLength(255)]
    public string Name { get; private set; }

    [MaxLength(255)]
    public string Slug { get; private set; }

    public string? Description { get; private set; }

    [MaxLength(100)]
    public string Location { get; private set; }

    public int? VenueCapacity { get; private set; }

    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    public DateTime SaleStartTime { get; private set; }
    public DateTime? SaleEndTime { get; private set; }

    public string? ImageUrl { get; private set; }

    [MaxLength(50)]
    public EventStatus Status { get; private set; } = EventStatus.DRAFT;

    public Guid OrganizerId { get; private set; }
    public User Organizer { get; private set; } = null!;

    public bool IsFeatured { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    private Event()
    {
        Name = string.Empty;
        Slug = string.Empty;
        Location = string.Empty;
    }

    /// <summary>
    /// Creates a new event.
    /// </summary>
    public Event(
        string name,
        string slug,
        string location,
        DateTime startTime,
        DateTime endTime,
        Guid organizerId,
        string? description = null,
        string? imageUrl = null,
        int? venueCapacity = null,
        DateTime? saleStartTime = null,
        DateTime? saleEndTime = null,
        bool isFeatured = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Event name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException("Event slug is required.", nameof(slug));
        if (string.IsNullOrWhiteSpace(location))
            throw new DomainException("Event location is required.", nameof(location));
        if (endTime <= startTime)
            throw new InValidDateException("Event end time must be after start time.");
        if (saleEndTime != null && saleStartTime != null && saleEndTime <= saleStartTime)
            throw new InValidDateException("Event sale end time must be after sale start time.");

        Name = name.Trim();
        Slug = slug.ToLowerInvariant().Trim();
        Location = location.Trim();
        StartTime = startTime;
        EndTime = endTime;
        OrganizerId = organizerId;
        Description = description;
        ImageUrl = imageUrl;
        VenueCapacity = venueCapacity;
        SaleStartTime = saleStartTime ?? DateTime.UtcNow;
        SaleEndTime = saleEndTime;
        IsFeatured = isFeatured;
    }

    /// <summary>
    /// Updates the event's date range and validates it.
    /// </summary>
    public void UpdateDateRange(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
            throw new InValidDateException("Event end time must be after start time.");
        StartTime = startTime;
        EndTime = endTime;
        Touch();
    }

    /// <summary>
    /// Updates the event's sale period and validates it.
    /// </summary>
    public void UpdateSalePeriod(DateTime saleStartTime, DateTime? saleEndTime)
    {
        if (saleEndTime != null && saleEndTime <= saleStartTime)
            throw new InValidDateException("Event sale end time must be after sale start time.");
        SaleStartTime = saleStartTime;
        SaleEndTime = saleEndTime;
        Touch();
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;

    /// <summary>
    /// Publishes the event.
    /// </summary>
    public void Publish()
    {
        if (Status == EventStatus.CANCELLED)
            throw new InValidOperationException("Cannot publish a cancelled event.");
        Status = EventStatus.PUBLISHED;
        Touch();
    }

    /// <summary>
    /// Marks the event as completed.
    /// </summary>
    public void MarkCompleted()
    {
        if (Status != EventStatus.PUBLISHED)
            throw new InValidOperationException("Only published events can be marked as completed.");
        Status = EventStatus.COMPLETED;
        Touch();
    }
}

/// <summary>
/// Status of an event.
/// </summary>
public enum EventStatus
{
    DRAFT,
    PUBLISHED,
    CANCELLED,
    COMPLETED
}