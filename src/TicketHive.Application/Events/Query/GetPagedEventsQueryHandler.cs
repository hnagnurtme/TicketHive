using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common;
using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Events;

public class GetPagedEventsQueryHandler : IRequestHandler<GetPagedEventsQuery, ErrorOr<PagedResult<EventResult>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPagedEventsQueryHandler> _logger;

    public GetPagedEventsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPagedEventsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<PagedResult<EventResult>>> Handle(GetPagedEventsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting paged events - Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);

        var pagedResult = await _unitOfWork.Events.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.Status,
            request.IsFeatured,
            request.SearchTerm,
            request.StartDateFrom,
            request.StartDateTo,
            cancellationToken);

        var eventResults = pagedResult.Items?.Select(eventEntity => new EventResult(
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
        )).ToList() ?? new List<EventResult>();

        var result = new PagedResult<EventResult>
        {
            Items = eventResults,
            TotalItems = pagedResult.TotalItems,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize
        };

        _logger.LogInformation("Successfully retrieved {Count} events on page {PageNumber}", eventResults.Count, request.PageNumber);

        return result;
    }
}