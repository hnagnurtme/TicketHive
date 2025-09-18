using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TicketHive.Api.Common.Helpers
{

    public static class BaseControllerHelpers
    {
        public static IActionResult HandleResult<T>(T result, int statusCode, string? message = null)
        {
            if (result == null)
                return new StatusCodeResult(204);

            var response = new ApiResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = statusCode,
                Data = result,
                Meta = new { traceId = Activity.Current?.Id ?? "" }
            };

            return new ObjectResult(response) { StatusCode = statusCode };
        }
    }
}