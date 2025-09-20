namespace TicketHive.Domain.Exceptions;

using System.Net;
using TicketHive.Domain.Exceptions.Base;

public class DuplicateEmailException : DomainException
{
    public DuplicateEmailException(string message = "Email already in use")
        : base(message, ExceptionCode.DUPLICATE_EMAIL, HttpStatusCode.Conflict)
    {
    }
}