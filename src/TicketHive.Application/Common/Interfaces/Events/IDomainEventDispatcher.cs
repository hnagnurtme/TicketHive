using TicketHive.Domain.Common.Events;

namespace TicketHive.Application.Common.Interfaces.Events;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
