using ErrorOr;
using MediatR;

namespace TicketHive.Application.Tickets;

public record GetTicketsByEventIdQuery(Guid EventId) : IRequest<ErrorOr<List<TicketResult>>>;