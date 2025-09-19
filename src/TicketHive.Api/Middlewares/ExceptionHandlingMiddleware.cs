using System.Net;
using System.Text.Json;
using TicketHive.Api.Common;
using TicketHive.Domain.Exceptions.Base;

namespace TicketHive.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        string errorCode;
        string message;

        switch (exception)
        {
            case DomainException domainEx:
                statusCode = domainEx.StatusCode;
                errorCode = domainEx.Code;
                message = domainEx.Message;
                _logger.LogWarning("Domain exception: {Code} - {Message}", errorCode, message);
                break;

            case JsonException jsonEx:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "JSON_ERROR";
                message = jsonEx.Message;

                _logger.LogWarning("Json parsing error: {Message}", message);
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                errorCode = "INTERNAL_ERROR";
                message = "An unexpected error occurred.";

                _logger.LogError(exception, "Unexpected system error");
                break;
        }

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message,
            StatusCode = (int)statusCode,
            Data = null,
            Meta = null,
            ErrorCode = errorCode
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
