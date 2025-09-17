
using Microsoft.AspNetCore.Mvc;

using ErrorOr;
using TicketHive.Application.Common;
namespace TicketHive.Api.Common;
public static class BaseControllerHelpers
{
    public static IActionResult HandleResult<T>(ErrorOr<T> result, int StatusCode, string? message = null)
    {
        if (result.IsError)
            return HandleError<T>(result.Errors);

        var value = result.Value;

        if (value == null)
            return new StatusCodeResult(204); 

        var response = new ApiResponse<T>
        {
            Success = true,
            Message = message,
            StatusCode = StatusCode,
            Data = value
        };

        return new ObjectResult(response) { StatusCode = StatusCode };
    }

    public static IActionResult HandlePagedResult<T>(ErrorOr<PagedResult<T>> result, int StatusCode = 200, string? message = null)
    {
        if (result.IsError)
            return HandleError<PagedResult<T>>(result.Errors);

        var paged = result.Value;

        if (paged == null || paged.Items == null || !paged.Items.Any())
            return new StatusCodeResult(204); 

        var meta = new
        {
            paged.PageNumber,
            paged.PageSize,
            paged.TotalItems,
            paged.TotalPages
        };

        var response = new ApiResponse<PagedResult<T>>
        {
            Success = true,
            Message = message,
            StatusCode = StatusCode,
            Data = paged,
            Meta = meta
        };

        return new ObjectResult(response) { StatusCode = StatusCode };
    }

    private static IActionResult HandleError<T>(List<Error> errors)
    {
        var firstError = errors.First();
        var code = firstError.Type switch
        {
            ErrorType.NotFound => 404,
            ErrorType.Conflict => 409,
            ErrorType.Validation => 400,
            _ => 500
        };
        return new ObjectResult(new ApiResponse<T>
        {
            Success = false,
            Message = firstError.Description,
            StatusCode = code,
            Data = default
        })
        { StatusCode = code };
    }
}
