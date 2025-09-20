using System.Security.Claims;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Domain.Events;


public class UserRegisteredDomainEventHandler 
    : INotificationHandler<UserRegisteredDomainEvent>
{
    private readonly ILogger<UserRegisteredDomainEventHandler> _logger;

    private readonly IEmailSender _emailSender;

    private readonly IJwtService _jwtService;

    public UserRegisteredDomainEventHandler(
        ILogger<UserRegisteredDomainEventHandler> logger,
        IJwtService jwtService,
        IEmailSender emailSender)
    {
        _logger = logger;
        _jwtService = jwtService;
        _emailSender = emailSender;
    }
    public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User registered with ID: {UserId} and Email: {Email}", 
            notification.UserId, notification.Email);

        // Generate email verification token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, notification.Email)
        };
        var token = _jwtService.GenerateToken(claims, TimeSpan.FromMinutes(15));
        // Gửi email verification (giả sử bạn đã cấu hình IEmailSender)

        var verificationLink = $"https://localhost:5000/api/auth/verify-email?Email={notification.Email}&Token={token}";
        var subject = "Welcome to TicketHive!";
        var message = $"Thank you for registering, please verify your email: {notification.Email } by clicking the link: {verificationLink}";
        _emailSender.SendEmailAsync(notification.Email, subject, message);

        return Task.CompletedTask;
    }
}
