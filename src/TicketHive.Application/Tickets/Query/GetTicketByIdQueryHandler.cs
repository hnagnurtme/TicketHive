using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Exceptions;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Common;

namespace TicketHive.Application.Tickets;

public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, ErrorOr<TicketDetailResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTicketByIdQueryHandler> _logger;

    public GetTicketByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetTicketByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<TicketDetailResult>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting ticket with ID: {TicketId}", request.TicketId);

        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId, cancellationToken);

        if (ticket == null)
        {
            _logger.LogInformation("Ticket not found with ID: {TicketId}", request.TicketId);
            throw new NotFoundException("Ticket not found");
        }

        _logger.LogInformation("Successfully retrieved ticket with ID: {TicketId}", request.TicketId);

        var remainingQuantity = ticket.Inventory?.RemainingQuantity ?? 0;

        return new TicketDetailResult(
            ticket.Id,
            ticket.EventId,
            ticket.Type,
            ticket.Name,
            ticket.Price,
            ticket.TotalQuantity,
            remainingQuantity,
            ticket.MinPurchase,
            ticket.MaxPurchase,
            ticket.Description,
            ticket.OriginalPrice,
            ticket.SaleStartTime,
            ticket.SaleEndTime,
            ticket.IsActive,
            ticket.SortOrder,
            ticket.CreatedAt,
            ticket.UpdatedAt,
            ticket.CreatedBy,
            ticket.UpdatedBy
        );
    }
}