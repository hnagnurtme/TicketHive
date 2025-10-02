using ErrorOr;
using MediatR;

namespace TicketHive.Application.Events;

public record DeleteEventCommand(Guid EventId) : IRequest<ErrorOr<bool>>;