using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Events;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, ErrorOr<EventDetailResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetEventByIdQueryHandler> _logger;

    public GetEventByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetEventByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<EventDetailResult>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting event with ID: {EventId}", request.EventId);

        var eventEntity = await _unitOfWork.Events.GetByIdAsync(request.EventId, cancellationToken);

        if (eventEntity == null)
        {
            _logger.LogInformation("Event not found with ID: {EventId}", request.EventId);
            return Error.NotFound("Event.NotFound", "Event not found");
        }

        _logger.LogInformation("Successfully retrieved event with ID: {EventId}", request.EventId);

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
            null // No UpdatedBy field in current Event entity
        );
    }
}