using TicketHive.Application.Common;

namespace TicketHive.Api.Contracts.Tickets;

public class GetTicketsResponse : PagedResult<TicketSummary>
{
    public GetTicketsResponse()
    {
    }

    public GetTicketsResponse(IEnumerable<TicketSummary> items, int totalItems, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}