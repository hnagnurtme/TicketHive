namespace TicketHive.Domain.Exceptions;

using System.Net;
using TicketHive.Domain.Exceptions.Base;

public class UnAuthorizationException : DomainException
{

    public UnAuthorizationException(string message = "Invalid credentials")
        : base(message, ExceptionCode.UN_AUTHORIZED, HttpStatusCode.Unauthorized)
    {
    }
}