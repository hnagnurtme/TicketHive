namespace TicketHive.Application.Common.Exceptions;

using System.Net;
using TicketHive.Application.Common.Exceptions.Base;

public class InvalidFormatException : ApplicationExceptions
{
    public InvalidFormatException(string message = "Invalid format")
        : base(message, ExceptionCode.INVALID_FORMAT, HttpStatusCode.BadRequest)
    {
    }
}