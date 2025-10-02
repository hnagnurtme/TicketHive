using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Events;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, ErrorOr<EventDetailResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateEventCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public UpdateEventCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateEventCommandHandler> logger, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<EventDetailResult>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
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

        _logger.LogInformation("Updating event with ID: {EventId} by user: {UserId}", request.EventId, currentUserId);

        if (request.StartTime != eventEntity.StartTime || request.EndTime != eventEntity.EndTime)
        {
            eventEntity.UpdateDateRange(request.StartTime, request.EndTime);
        }

        if (request.SaleStartTime.HasValue && 
            (request.SaleStartTime.Value != eventEntity.SaleStartTime || 
             request.SaleEndTime != eventEntity.SaleEndTime))
        {
            eventEntity.UpdateSalePeriod(request.SaleStartTime.Value, request.SaleEndTime);
        }

        _unitOfWork.Events.Update(eventEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated event with ID: {EventId}", request.EventId);

        return new EventDetailResult(
            eventEntity.Id,
            eventEntity.Name,
            eventEntity.Slug,
            eventEntity.Description,
            eventEntity.Location,
            eventEntity.StartTime,
            eventEntity.EndTime,
            eventEntity.VenueCapacity,
            eventEntity.SaleStartTime,
            eventEntity.SaleEndTime,
            eventEntity.ImageUrl,
            eventEntity.IsFeatured,
            eventEntity.OrganizerId,
            eventEntity.Status.ToString(),
            eventEntity.CreatedAt,
            eventEntity.UpdatedAt,
            eventEntity.OrganizerId, // Using OrganizerId as CreatedBy
            currentUserId // Using current user as UpdatedBy
        );
    }
}