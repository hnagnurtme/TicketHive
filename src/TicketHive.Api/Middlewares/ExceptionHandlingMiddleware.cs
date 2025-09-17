using System.Net;
using System.Text.Json;
using TicketHive.Api.Common;
using TicketHive.Domain.Exceptions;
using TicketHive.Domain.Exceptions.Base;

namespace TicketHive.Api.Middlewares
{
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
            catch (DuplicateEmailException ex)
            {
                _logger.LogWarning("Duplicate email exception occurred: {Message}", ex.Message);
                await HandleDuplicateEmailExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleDuplicateEmailExceptionAsync(HttpContext context, DuplicateEmailException exception)
        {
            var response = new ApiResponse<object>
            {
                Success = false,
                Message = exception.Message,
                StatusCode = (int)HttpStatusCode.Conflict,
                Data = null,
                Meta = null,
                ErrorCode = exception.Code
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, errorCode, message) = exception switch
            {
                // Xử lý các ngoại lệ domain khác (nếu có DomainException)
                DomainException domainEx => (
                    HttpStatusCode.BadRequest,
                    "DOMAIN_ERROR",
                    domainEx.Message
                ),
                // Xử lý lỗi xác thực JSON (nếu xảy ra ngoài ValidationFilter)
                System.Text.Json.JsonException jsonEx => (
                    HttpStatusCode.BadRequest,
                    "JSON_ERROR",
                    jsonEx.Message
                ),
                // Xử lý các ngoại lệ không xác định
                _ => (
                    HttpStatusCode.InternalServerError,
                    "INTERNAL_ERROR",
                    "An unexpected error occurred."
                )
            };

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
            return context.Response.WriteAsync(json);
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}