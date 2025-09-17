using ErrorOr;
using Microsoft.AspNetCore.Mvc;
namespace TicketHive.Api.Common;

public static class OK
{
    public static IActionResult HandleResult<T>(ErrorOr<T> result, string? message = null)
        => BaseControllerHelpers.HandleResult(result, statusCode: 200, message);
}

public static class CREATE
{
    public static IActionResult HandleResult<T>(ErrorOr<T> result, string? message = null)
        => BaseControllerHelpers.HandleResult(result, statusCode: 201, message);
}

public static class NO_CONTENT
{
    public static IActionResult HandleResult<T>(ErrorOr<T> result)
        => BaseControllerHelpers.HandleResult(result, statusCode: 204);
}


