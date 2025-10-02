using ErrorOr;
using MediatR;
using TicketHive.Application.Common;

namespace TicketHive.Application.Tickets;

public record GetTicketsQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm,
    Guid? EventId,
    bool? IsActive,
    string? SortBy,
    string? SortDirection
) : IRequest<ErrorOr<PagedResult<TicketResult>>>;