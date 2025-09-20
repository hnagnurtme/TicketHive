using MediatR;
using TicketHive.Domain.Common.Events;

namespace TicketHive.Domain.Events;

public record UserRegisteredDomainEvent(Guid UserId, string Email) 
    : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
