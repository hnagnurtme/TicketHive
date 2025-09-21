namespace TicketHive.Api.Contracts.Events;

public class PublishEventRequest
{
    public required Guid EventId { get; set; }
}