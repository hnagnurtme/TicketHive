namespace TicketHive.Domain.Exceptions;

using System.Net;
using TicketHive.Domain.Exceptions.Base;

public class TokenAlreadyUsedException : DomainException
{

    public TokenAlreadyUsedException(string message = "Invalid credentials")
        : base(message, ExceptionCode.TOKEN_ALREADY_USED, HttpStatusCode.Unauthorized)
    {
    }
}