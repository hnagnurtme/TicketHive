using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TicketHive.Api.Common.Helpers;

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

    public static IActionResult HandleResult<T>(ErrorOr<T> result, string? successMessage = null)
    {
        if (!result.IsError)
        {
            var response = new ApiResponse<T>
            {
                Success = true,
                Message = successMessage ?? "Success",
                StatusCode = 200,
                Data = result.Value,
                Meta = new { traceId = Activity.Current?.Id ?? "" }
            };

            return new OkObjectResult(response);
        }

        var error = result.FirstError;
        var statusCode = MapStatusCode(error.Type);

        var errorResponse = new ApiResponse<object>
        {
            Success = false,
            Message = error.Description,
            StatusCode = statusCode,
            ErrorCode = error.Code,
            Meta = new { traceId = Activity.Current?.Id ?? "" }
        };

        return new ObjectResult(errorResponse) { StatusCode = statusCode };
    }

    private static int MapStatusCode(ErrorType type)
    {
        return type switch
        {
            ErrorType.NotFound => 404,
            ErrorType.Validation => 400,
            ErrorType.Unauthorized => 401,
            ErrorType.Forbidden => 403,
            ErrorType.Conflict => 409,
            _ => 500
        };
    }
}

