namespace TicketHive.Domain.Exceptions.Base;

using System.Net;

public class DomainException : Exception
{
    public string Code { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public DomainException(string message, string code, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
    {
        Code = code;
        StatusCode = statusCode;
    }
}