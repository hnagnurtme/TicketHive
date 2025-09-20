namespace TicketHive.Application.Common.Exceptions;

using System.Net;
using TicketHive.Application.Common.Exceptions.Base;

public class DuplicateEmailException : ApplicationExceptions
{
    public DuplicateEmailException(string message = "Email already in use")
        : base(message, ExceptionCode.DUPLICATE_EMAIL, HttpStatusCode.Conflict)
    {
    }
}