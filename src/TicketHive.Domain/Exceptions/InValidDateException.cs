namespace TicketHive.Domain.Exceptions;

using System.Net;
using TicketHive.Domain.Exceptions.Base;


public class InValidDateException : DomainException
{

    public InValidDateException(string message = "Invalid date provided")
        : base(message, ExceptionCode.INVALID_DATE, HttpStatusCode.BadRequest)
    {
    }
}