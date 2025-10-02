using ErrorOr;
using MediatR;

namespace TicketHive.Application.Events;

public record GetAllEventsQuery() : IRequest<ErrorOr<List<EventResult>>>;