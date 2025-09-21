using MediatR;
using ErrorOr;
namespace TicketHive.Application.Events.Command;

public record PushlishEventCommand(
    Guid EventId
) : IRequest<ErrorOr<PublishEventResult>>;