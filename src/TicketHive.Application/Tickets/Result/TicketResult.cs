namespace TicketHive.Application.Tickets;

public record TicketResult(Guid TicketId ,Guid EventId, string Type, string Name , decimal Price, int TotalQuantity, int RemainingQuantity, int MinPurchase, int MaxPurchase, string? Description, decimal? OriginalPrice, DateTime? SaleStartTime, DateTime? SaleEndTime, bool IsActive, int SortOrder);