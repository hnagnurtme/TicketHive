using MediatR;
namespace TicketHive.Application.Ticket.Command;

using ErrorOr;
using TicketHive.Domain.Entities;
using TicketHive.Application.Common.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Tickets;
using TicketHive.Application.Common.Interfaces.Events;

public class AddTicketCommandHandler : IRequestHandler<CreateTicketCommand, ErrorOr<TicketResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<AddTicketCommandHandler> _logger;

    private readonly IDomainEventDispatcher _domainEventDispatcher;
    public AddTicketCommandHandler(IUnitOfWork unitOfWork, ILogger<AddTicketCommandHandler> logger, IDomainEventDispatcher domainEventDispatcher)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<ErrorOr<TicketResult>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new Ticket(
            eventId: request.EventId,
            type: request.Type,
            name: request.Name,
            price: request.Price,
            totalQuantity: request.TotalQuantity,
            minPurchase: request.MinPurchase,
            maxPurchase: request.MaxPurchase,
            description: request.Description,
            originalPrice: request.OriginalPrice,
            saleStartTime: request.SaleStartTime,
            saleEndTime: request.SaleEndTime,
            isActive: request.IsActive,
            sortOrder: request.SortOrder
        );

        await _unitOfWork.Tickets.AddAsync(ticket, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _domainEventDispatcher.DispatchEventsAsync(ticket.DomainEvents);
        ticket.ClearDomainEvents();


        var result = new TicketResult(
            Guid.NewGuid(),
            request.Name,
            request.Price,
            request.TotalQuantity,
            request.TotalQuantity,
            request.MinPurchase,
            request.MaxPurchase,
            request.Description,
            request.OriginalPrice,
            request.SaleStartTime,
            request.SaleEndTime,
            request.IsActive,
            request.SortOrder
        );

        _logger.LogInformation("Ticket created with ID: {TicketId}", result.TicketId);

        return result;
    }
}



