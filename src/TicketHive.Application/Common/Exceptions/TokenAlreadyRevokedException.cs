namespace TicketHive.Application.Common.Exceptions;

using System.Net;
using TicketHive.Application.Common.Exceptions.Base;

public class TokenAlreadyRevokedException : ApplicationExceptions
{

    public TokenAlreadyRevokedException(string message = "Invalid credentials")
        : base(message, ExceptionCode.TOKEN_ALREADY_REVOKED, HttpStatusCode.Unauthorized)
    {
    }
}