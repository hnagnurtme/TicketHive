namespace TicketHive.Domain.Exceptions;

using System.Net;
using TicketHive.Domain.Exceptions.Base;

public class TokenAlreadyRevokedException : DomainException
{

    public TokenAlreadyRevokedException(string message = "Invalid credentials")
        : base(message, ExceptionCode.TOKEN_ALREADY_REVOKED, HttpStatusCode.Unauthorized)
    {
    }
}