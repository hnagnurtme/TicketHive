using ErrorOr;
using MediatR;

namespace TicketHive.Application.Tickets;

public record DeleteTicketCommand(Guid TicketId) : IRequest<ErrorOr<bool>>;