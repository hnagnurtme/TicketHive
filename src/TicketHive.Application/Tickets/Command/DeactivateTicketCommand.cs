using ErrorOr;
using MediatR;

namespace TicketHive.Application.Tickets;

public record DeactivateTicketCommand(Guid TicketId, Guid DeactivatedBy) : IRequest<ErrorOr<bool>>;