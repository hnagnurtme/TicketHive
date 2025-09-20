using Microsoft.Extensions.Configuration;
using MimeKit;
using TicketHive.Application.Common.Interfaces;
using MailKit.Net.Smtp;
namespace TicketHive.Infrastructure.ExternalServices;
using Microsoft.Extensions.Logging;
using TicketHive.Infrastructure.Utils;

public class EmailSenderService : IEmailSender
{
    private readonly IConfiguration _configuration;

    private readonly ILogger<EmailSenderService> _logger;


    public EmailSenderService(IConfiguration configuration, ILogger<EmailSenderService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var fromAddress = LoadProperty.GetProperty(_configuration, "Smtp:From");
        var host = LoadProperty.GetProperty(_configuration, "Smtp:Host");
        var portString = LoadProperty.GetProperty(_configuration, "Smtp:Port");
        var user = LoadProperty.GetProperty(_configuration, "Smtp:User");
        var password = LoadProperty.GetProperty(_configuration, "Smtp:Password");

        if (!int.TryParse(portString, out var port))
        {
            _logger.LogError("Invalid SMTP Port configuration: {Port}", portString);
            throw new InvalidOperationException("SMTP port configuration is missing or invalid.");
        }

        Console.WriteLine($"[EmailService] Preparing email to {to} with subject '{subject}'");

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(user, fromAddress));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart("html")
        {
            Text = body
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(fromAddress, password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);

        Console.WriteLine($"[EmailService] Email sent to {to} successfully.");
    }
}