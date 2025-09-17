using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using TicketHive.Api.Common;

namespace TicketHive.Api.Filters
{
    public class ValidationFilter : IActionFilter
    {
        private readonly ILogger<ValidationFilter> _logger;

        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value != null)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => new
                    {
                        ErrorCode = "VALIDATION_ERROR",
                        Message = x.ErrorMessage
                    })
                    .ToList();

                if (errors.Any())
                {
                    _logger.LogWarning("Model validation failed: {Errors}",
                        string.Join("; ", errors.Select(e => e.Message)));

                    var response = new ApiResponse<object>
                    {
                        Success = false,
                        Message = errors.First().Message,
                        StatusCode = 400,
                        Data = null,
                        Meta = null,
                        ErrorCode = errors.First().ErrorCode
                    };

                    context.Result = new ObjectResult(response) { StatusCode = 400 };
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Xử lý lỗi deserialization hoặc ngoại lệ trong action
            if (context.Exception != null && context.Exception is System.Text.Json.JsonException jsonEx)
            {
                _logger.LogWarning("JSON deserialization error: {Message}", jsonEx.Message);

                var response = new ApiResponse<object>
                {
                    Success = false,
                    Message = jsonEx.Message,
                    StatusCode = 400,
                    Data = null,
                    Meta = null,
                    ErrorCode = "JSON_ERROR"
                };

                context.Result = new ObjectResult(response) { StatusCode = 400 };
                context.ExceptionHandled = true; 
            }
        }
    }
}