namespace TicketHive.Domain.Exceptions;

using System.Net;
using TicketHive.Application.Common.Exceptions.Base;

public class EmailNotVerifyException : ApplicationExceptions
{
    public EmailNotVerifyException(string message = "Email not verified")
        : base(message, ExceptionCode.EMAIL_NOT_VERIFIED, HttpStatusCode.Unauthorized)
    {
    }
}