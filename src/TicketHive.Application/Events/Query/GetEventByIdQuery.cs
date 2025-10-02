using ErrorOr;
using MediatR;

namespace TicketHive.Application.Events;

public record GetEventByIdQuery(Guid EventId) : IRequest<ErrorOr<EventDetailResult>>;