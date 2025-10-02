using ErrorOr;
using MediatR;

namespace TicketHive.Application.Tickets;

public record DeactivateTicketCommand(Guid TicketId) : IRequest<ErrorOr<bool>>;