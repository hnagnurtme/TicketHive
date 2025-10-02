using MediatR;
namespace TicketHive.Application.Ticket.Command;

using ErrorOr;
using TicketHive.Domain.Entities;
using TicketHive.Application.Common.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Tickets;
using TicketHive.Application.Common.Interfaces.Events;
using TicketHive.Application.Common.Exceptions;
using TicketHive.Application.Common.Interfaces;

public class AddTicketCommandHandler : IRequestHandler<CreateTicketCommand, ErrorOr<TicketResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<AddTicketCommandHandler> _logger;

    private readonly ICurrentUserService _currentUserService;

    private readonly IDomainEventDispatcher _domainEventDispatcher;
    public AddTicketCommandHandler(IUnitOfWork unitOfWork, ILogger<AddTicketCommandHandler> logger, IDomainEventDispatcher domainEventDispatcher, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _domainEventDispatcher = domainEventDispatcher;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<TicketResult>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var eventExists = await _unitOfWork.Events.ExistsAsync(request.EventId, cancellationToken);
        if (!eventExists)
        {
            throw new NotFoundException("Event not found");

        }
        var createdBy = _currentUserService.UserId;
        if (createdBy == Guid.Empty)
        {
            throw new UnAuthorizationException("User not authorized");
        }

        var ticket = new Ticket(
            request.EventId,
            request.Type,
            request.Name,
            request.Price,
            request.TotalQuantity,
            createdBy,                 
            request.MinPurchase,
            request.MaxPurchase,
            request.Description,
            request.OriginalPrice,
            request.SaleStartTime,
            request.SaleEndTime,
            request.IsActive,
            request.SortOrder
        );


        await _unitOfWork.Tickets.AddAsync(ticket, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _domainEventDispatcher.DispatchEventsAsync(ticket.DomainEvents);
        ticket.ClearDomainEvents();


        var result = new TicketResult(
            ticket.Id,
            ticket.EventId,
            ticket.Type,
            ticket.Name,
            ticket.Price,
            ticket.TotalQuantity,
            ticket.Inventory.RemainingQuantity,
            ticket.MinPurchase,
            ticket.MaxPurchase,
            ticket.Description,
            ticket.OriginalPrice,
            ticket.SaleStartTime,
            ticket.SaleEndTime,
            ticket.IsActive,
            ticket.SortOrder
        );


        _logger.LogInformation("Ticket created with ID: {TicketId}", result.TicketId);

        return result;
    }
}



