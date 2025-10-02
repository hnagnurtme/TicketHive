namespace TicketHive.Api.Contracts.Events;

public class GetPagedEventsRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; }
    public bool? IsFeatured { get; set; }
    public string? SearchTerm { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
}