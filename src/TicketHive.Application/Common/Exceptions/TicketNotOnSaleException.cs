namespace TicketHive.Application.Exceptions;

using System.Net;
using TicketHive.Application.Common.Exceptions.Base;

public class TicketNotOnSaleException : ApplicationExceptions
{
    public TicketNotOnSaleException(string message = "Ticket is not on sale")
        : base(message, ExceptionCode.TICKET_NOT_ON_SALE, HttpStatusCode.BadRequest)
    {
    }
}