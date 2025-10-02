using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common;
using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Tickets;

public class GetTicketsQueryHandler : IRequestHandler<GetTicketsQuery, ErrorOr<PagedResult<TicketResult>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTicketsQueryHandler> _logger;

    public GetTicketsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetTicketsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<PagedResult<TicketResult>>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tickets with pagination - Page: {PageNumber}, Size: {PageSize}, SearchTerm: {SearchTerm}", 
            request.PageNumber, request.PageSize, request.SearchTerm);

        var (tickets, totalCount) = await _unitOfWork.Tickets.GetPagedAsync(
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            searchTerm: request.SearchTerm,
            eventId: request.EventId,
            isActive: request.IsActive,
            sortBy: request.SortBy,
            sortDirection: request.SortDirection,
            cancellationToken: cancellationToken
        );

        var ticketResults = tickets.Select(ticket => new TicketResult(
            ticket.Id,
            ticket.EventId,
            ticket.Type,
            ticket.Name,
            ticket.Price,
            ticket.TotalQuantity,
            ticket.Inventory?.RemainingQuantity ?? 0,
            ticket.MinPurchase,
            ticket.MaxPurchase,
            ticket.Description,
            ticket.OriginalPrice,
            ticket.SaleStartTime,
            ticket.SaleEndTime,
            ticket.IsActive,
            ticket.SortOrder
        )).ToList();

        var pagedResult = new PagedResult<TicketResult>
        {
            Items = ticketResults,
            TotalItems = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        _logger.LogInformation("Successfully retrieved {Count} tickets out of {TotalCount} total tickets", 
            ticketResults.Count, totalCount);

        return pagedResult;
    }
}