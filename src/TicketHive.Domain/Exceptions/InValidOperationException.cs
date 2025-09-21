namespace TicketHive.Domain.Exceptions;

using System.Net;
using TicketHive.Domain.Exceptions.Base;

public class InValidOperationException : DomainException
{

    public InValidOperationException(string message = "Invalid operation")
        : base(message, ExceptionCode.INVALID_OPERATION, HttpStatusCode.BadRequest)
    {
    }
}