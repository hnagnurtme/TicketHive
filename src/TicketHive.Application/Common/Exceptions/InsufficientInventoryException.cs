namespace TicketHive.Application.Exceptions;

using System.Net;
using TicketHive.Application.Common.Exceptions.Base;

public class InsufficientInventoryException : ApplicationExceptions
{
    public InsufficientInventoryException(string message = "Insufficient inventory for the requested ticket.")
            : base(message, ExceptionCode.INSUFFICIENT_INVENTORY, HttpStatusCode.BadRequest)
    {
    }
}