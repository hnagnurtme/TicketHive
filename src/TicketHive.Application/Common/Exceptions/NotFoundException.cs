namespace TicketHive.Application.Common.Exceptions;

using System.Net;
using TicketHive.Application.Common.Exceptions.Base;

public class NotFoundException : ApplicationExceptions
{
    public NotFoundException(string message = "Resource not found")
        : base(message, ExceptionCode.NOT_FOUND, HttpStatusCode.BadRequest)
    {
    }
}