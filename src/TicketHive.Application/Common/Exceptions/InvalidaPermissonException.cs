namespace TicketHive.Application.Exceptions;

using System.Net;
using TicketHive.Application.Common.Exceptions.Base;

public class InvalidPermissionException : ApplicationExceptions
{
    public InvalidPermissionException(string message = "You do not have permission to perform this action.")
        : base(message, ExceptionCode.INVALID_PERMISSION, HttpStatusCode.Forbidden)
    {
    }
}