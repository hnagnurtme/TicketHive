        using ErrorOr;
using MediatR;

namespace TicketHive.Application.Tickets;

public record GetTicketByIdQuery(Guid TicketId) : IRequest<ErrorOr<TicketDetailResult>>;