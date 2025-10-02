using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Events;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, ErrorOr<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteEventCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public DeleteEventCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteEventCommandHandler> logger, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (currentUserId == Guid.Empty)
        {
            _logger.LogWarning("Invalid user ID from current user service");
            return Error.Unauthorized("User.Unauthorized", "User not authorized");
        }

        var eventEntity = await _unitOfWork.Events.GetByIdAsync(request.EventId, cancellationToken);

        if (eventEntity == null)
        {
            _logger.LogInformation("Event not found with ID: {EventId}", request.EventId);
            return Error.NotFound("Event.NotFound", "Event not found");
        }

        // Check if event has tickets
        var hasTickets = await _unitOfWork.Events.HasTicketsAsync(request.EventId, cancellationToken);
        if (hasTickets)
        {
            _logger.LogWarning("Cannot delete event with ID: {EventId} - has existing tickets", request.EventId);
            return Error.Conflict("Event.HasTickets", "Cannot delete event with existing tickets");
        }

        _logger.LogInformation("Deleting event with ID: {EventId} by user: {UserId}", request.EventId, currentUserId);

        _unitOfWork.Events.Delete(eventEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully deleted event with ID: {EventId}", request.EventId);

        return true;
    }
}