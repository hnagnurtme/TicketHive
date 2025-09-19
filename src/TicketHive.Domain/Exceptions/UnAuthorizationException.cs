namespace TicketHive.Domain.Exceptions;

using System.Net;
using TicketHive.Domain.Exceptions.Base;

public class UnAuthorizationException : DomainException
{
    public const string unAuthor = "UN_AUTHORIZED";

    public UnAuthorizationException(string message = "Invalid credentials")
        : base(message, unAuthor, HttpStatusCode.Unauthorized)
    {
    }
}