using MediatR;
using TicketHive.Domain.Events;

public class UserRegisteredDomainEventHandler 
    : INotificationHandler<UserRegisteredDomainEvent>
{
    public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT HANDLER] User registered: {notification.Email}");
        return Task.CompletedTask;
    }
}
