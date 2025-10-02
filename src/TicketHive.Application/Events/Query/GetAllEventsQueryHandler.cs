using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Events;

public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, ErrorOr<List<EventResult>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllEventsQueryHandler> _logger;

    public GetAllEventsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllEventsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<List<EventResult>>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all events");

        var events = await _unitOfWork.Events.GetAllAsync(cancellationToken);

        var eventResults = events.Select(eventEntity => new EventResult(
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
            eventEntity.Status.ToString()
        )).ToList();

        _logger.LogInformation("Successfully retrieved {Count} events", eventResults.Count);

        return eventResults;
    }
}