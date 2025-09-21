namespace TicketHive.Api.Contracts.Events;

public class AddEventRequest
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public string Location { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int? VenueCapacity { get; set; }
    public DateTime? SaleStartTime { get; set; }
    public DateTime? SaleEndTime { get; set; }

    public string? ImageUrl { get; set; }
    public bool IsFeatured { get; set; }
}
