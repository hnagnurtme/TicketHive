using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Tickets;

public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, ErrorOr<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTicketCommandHandler> _logger;

    public DeleteTicketCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteTicketCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId, cancellationToken);

        if (ticket == null)
        {
            _logger.LogInformation("Ticket not found with ID: {TicketId}", request.TicketId);
            return Error.NotFound("Ticket.NotFound", "Ticket not found");
        }

        // Check if ticket has any sales/bookings
        var hasSales = await _unitOfWork.Tickets.HasSalesAsync(request.TicketId, cancellationToken);
        if (hasSales)
        {
            _logger.LogWarning("Cannot delete ticket with ID: {TicketId} - has existing sales", request.TicketId);
            return Error.Conflict("Ticket.HasSales", "Cannot delete ticket with existing sales");
        }

        _logger.LogInformation("Deleting ticket with ID: {TicketId}", request.TicketId);

        _unitOfWork.Tickets.Delete(ticket);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully deleted ticket with ID: {TicketId}", request.TicketId);

        return true;
    }
}