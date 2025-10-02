using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Tickets;

public class GetTicketsByEventIdQueryHandler : IRequestHandler<GetTicketsByEventIdQuery, ErrorOr<List<TicketResult>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTicketsByEventIdQueryHandler> _logger;

    public GetTicketsByEventIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetTicketsByEventIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<List<TicketResult>>> Handle(GetTicketsByEventIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tickets for event ID: {EventId}", request.EventId);

        var tickets = await _unitOfWork.Tickets.GetByEventIdAsync(request.EventId, cancellationToken);

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

        _logger.LogInformation("Successfully retrieved {Count} tickets for event ID: {EventId}", ticketResults.Count, request.EventId);

        return ticketResults;
    }
}