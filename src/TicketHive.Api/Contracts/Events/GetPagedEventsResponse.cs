using TicketHive.Application.Common;

namespace TicketHive.Api.Contracts.Events;

public class GetPagedEventsResponse
{
    public PagedResult<EventSummary> Events { get; set; } = null!;
}