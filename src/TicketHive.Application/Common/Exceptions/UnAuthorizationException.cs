namespace TicketHive.Application.Common.Exceptions;

using System.Net;
using TicketHive.Application.Common.Exceptions.Base;

public class UnAuthorizationException : ApplicationExceptions
{
    public UnAuthorizationException(string message = "Invalid credentials")
        : base(message, ExceptionCode.UN_AUTHORIZED, HttpStatusCode.Unauthorized)
    {
    }
}