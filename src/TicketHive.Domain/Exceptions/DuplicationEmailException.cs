namespace TicketHive.Application.Exceptions;

public class DuplicateEmailException : Exception
{
    public string ErrorCode { get; } = "123333";

    public DuplicateEmailException(string message) : base(message)
    {
    }
}