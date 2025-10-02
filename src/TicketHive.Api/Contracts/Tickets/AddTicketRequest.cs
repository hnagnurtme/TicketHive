namespace TicketHive.Api.Contracts.Tickets;

public class AddTicketRequest
{
    public Guid EventId { get; set; }  
    public string Type { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int TotalQuantity { get; set; }
    public int MinPurchase { get; set; }
    public int MaxPurchase { get; set; }
    public string? Description { get; set; }
    public decimal? OriginalPrice { get; set; }
    public DateTime? SaleStartTime { get; set; }
    public DateTime? SaleEndTime { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
}