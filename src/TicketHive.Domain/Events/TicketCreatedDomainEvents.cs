using MediatR;
using TicketHive.Domain.Common.Events;

namespace TicketHive.Domain.Events;

public record TicketCreatedDomainEvent(Guid TicketId, Guid EventId, string TicketName, decimal Price, int TotalQuantity) 
    : IDomainEvent, INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}