using System.Security.Claims;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Domain.Events;
using Microsoft.Extensions.Configuration;

namespace TicketHive.Application.Ticket.Events;

public class TicketCreatedDomainEventHandler
    : INotificationHandler<TicketCreatedDomainEvent>
{
    private readonly ILogger<TicketCreatedDomainEventHandler> _logger;

    private readonly IEmailSender _emailSender;

    private readonly IConfiguration _configuration;

    public TicketCreatedDomainEventHandler(
        ILogger<TicketCreatedDomainEventHandler> logger,
        IConfiguration configuration,
        IEmailSender emailSender)
    {
        _logger = logger;
        _configuration = configuration;
        _emailSender = emailSender;
    }
    public Task Handle(TicketCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Ticket created with ID: {TicketId}, Name: {TicketName}, Price: {Price}, TotalQuantity: {TotalQuantity}",
            notification.TicketId, notification.TicketName, notification.Price, notification.TotalQuantity);

        // // Send email notification to admin
        // var adminEmail = _configuration["AdminSettings:AdminEmail"]
        //       ?? throw new InvalidOperationException("Admin email is not configured.");

        // var subject = "New Ticket Created";
        // var message = $"A new ticket has been created:\n\n" +
        //               $"Ticket ID: {notification.TicketId}\n" +
        //               $"Event ID: {notification.EventId}\n" +
        //               $"Ticket Name: {notification.TicketName}\n" +
        //               $"Price: {notification.Price}\n" +
        //               $"Total Quantity: {notification.TotalQuantity}\n";

        // _emailSender.SendEmailAsync(adminEmail, subject, message);

        return Task.CompletedTask;
    }
}