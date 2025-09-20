namespace TicketHive.Application.Common.Exceptions;

using System.Net;
using TicketHive.Application.Common.Exceptions.Base;
public class TokenAlreadyUsedException : ApplicationExceptions
{

    public TokenAlreadyUsedException(string message = "Invalid credentials")
        : base(message, ExceptionCode.TOKEN_ALREADY_USED, HttpStatusCode.Unauthorized)
    {
    }
}