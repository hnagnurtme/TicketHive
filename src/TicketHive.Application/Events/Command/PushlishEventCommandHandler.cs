namespace TicketHive.Application.Events.Command;

using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Constants;
using TicketHive.Application.Common.Exceptions;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Exceptions;
using TicketHive.Domain.Entities;

public class PushlishEventCommandHandler : IRequestHandler<PushlishEventCommand, ErrorOr<PublishEventResult>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PushlishEventCommandHandler> _logger;

    private readonly ICurrentUserService _currentUserService;

    public PushlishEventCommandHandler(IUnitOfWork unitOfWork, ILogger<PushlishEventCommandHandler> logger, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<PublishEventResult>> Handle(PushlishEventCommand request, CancellationToken cancellationToken)
    {
        // extract user info from currentUserService
        var userId = _currentUserService.UserId;
        var role = _currentUserService.Roles;

        _logger.LogInformation("User {UserId} with role {Role} is attempting to publish event {EventId}", userId, role, request.EventId);

        // extract eventid from request
        var eventId = request.EventId;

        // validate user info
        ValidateUserInfo(userId);

        // validate user permissions
        ValidateUserPermissions(role);

        // fetch event from db
        var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId, cancellationToken);

        // validate event
        if (eventEntity == null)
        {
            _logger.LogWarning("Event with ID {EventId} not found.", eventId);
            throw new NotFoundException("Event not found.");
        }
        if (eventEntity.OrganizerId != userId)
        {
            _logger.LogWarning("User {UserId} attempted to publish event {EventId} without permission.", userId, eventId);
            throw new InvalidPermissionException("You do not have permission to publish this event.");
        }
        if (eventEntity.Status != EventStatus.DRAFT)
        {
            _logger.LogWarning("Event {EventId} is not in DRAFT status and cannot be published.", eventId);
            throw new InvalidOperationException("Only events in DRAFT status can be published.");
        }
        // publish event
        eventEntity.Publish();
        _unitOfWork.Events.Update(eventEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        // return result

        var publishEventResult = new PublishEventResult(
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
            eventEntity.Status,
            eventEntity.CreatedAt
        );

        return publishEventResult;

    }
    private void ValidateUserInfo(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            _logger.LogWarning("Invalid user information: UserId={UserId}", userId);
            throw new UnAuthorizationException("User information is incomplete or invalid.");
        }
    }   
    private void ValidateUserPermissions(List<string> roles)
    {
        if (roles == null || !roles.Contains(RoleConstants.ORGANIZER))
        {
            _logger.LogWarning("User does not have the required permissions. Roles: {Roles}", roles);
            throw new UnAuthorizationException("You do not have permission to publish an event.");
        }
    }
}