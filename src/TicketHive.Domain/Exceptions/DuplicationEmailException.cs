namespace TicketHive.Domain.Exceptions;

using System.Net;
using TicketHive.Domain.Exceptions.Base;

public class DuplicateEmailException : DomainException
{
    public const string DuplicateEmailCode = "USER_EMAIL_DUPLICATE";

    public DuplicateEmailException(string message = "Email already in use")
        : base(message, DuplicateEmailCode, HttpStatusCode.Conflict)
    {
    }
}