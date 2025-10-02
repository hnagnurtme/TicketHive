using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Tickets;

public class DeactivateTicketCommandHandler : IRequestHandler<DeactivateTicketCommand, ErrorOr<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeactivateTicketCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public DeactivateTicketCommandHandler(IUnitOfWork unitOfWork, ILogger<DeactivateTicketCommandHandler> logger, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<bool>> Handle(DeactivateTicketCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (currentUserId == Guid.Empty)
        {
            _logger.LogWarning("Invalid user ID from current user service");
            return Error.Unauthorized("User.Unauthorized", "User not authorized");
        }

        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId, cancellationToken);

        if (ticket == null)
        {
            _logger.LogInformation("Ticket not found with ID: {TicketId}", request.TicketId);
           
            return Error.NotFound("Ticket.NotFound", "Ticket not found");
        }

        if (!ticket.IsActive)
        {
            _logger.LogInformation("Ticket with ID: {TicketId} is already deactivated", request.TicketId);
            return Error.Validation("Ticket.AlreadyDeactivated", "Ticket is already deactivated");
        }

        _logger.LogInformation("Deactivating ticket with ID: {TicketId} by user: {UserId}", request.TicketId, currentUserId);

        ticket.Deactivate(currentUserId);

        _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully deactivated ticket with ID: {TicketId}", request.TicketId);

        return true;
    }
}