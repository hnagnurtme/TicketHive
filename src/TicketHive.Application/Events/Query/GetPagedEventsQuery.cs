using ErrorOr;
using MediatR;
using TicketHive.Application.Common;

namespace TicketHive.Application.Events;

public record GetPagedEventsQuery(
    int PageNumber,
    int PageSize,
    string? Status,
    bool? IsFeatured,
    string? SearchTerm,
    DateTime? StartDateFrom,
    DateTime? StartDateTo
) : IRequest<ErrorOr<PagedResult<EventResult>>>;