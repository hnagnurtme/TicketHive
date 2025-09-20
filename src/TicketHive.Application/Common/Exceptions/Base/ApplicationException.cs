namespace TicketHive.Application.Common.Exceptions.Base;

using System.Net;

public class ApplicationExceptions : Exception
{
    public string Code { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public ApplicationExceptions(string message, string code, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
    {
        Code = code;
        StatusCode = statusCode;
    }
}