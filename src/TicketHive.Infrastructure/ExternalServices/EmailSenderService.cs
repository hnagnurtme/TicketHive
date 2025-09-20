

using TicketHive.Application.Common.Interfaces;

namespace TicketHive.Infrastructure.ExternalServices;

public class EmailSenderService : IEmailSender
{
    public Task SendEmailAsync(string to, string subject, string body)
    {
        Console.WriteLine($"Sending email to {to} with subject '{subject}' and body '{body}'");
        return Task.CompletedTask;
    }
}