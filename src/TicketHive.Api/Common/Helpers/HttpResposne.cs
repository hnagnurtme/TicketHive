using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using TicketHive.Api.Common.Helpers;

namespace TicketHive.Api.Common;

public static class OK
{
    // ErrorOr<T>
    public static IActionResult HandleResult<T>(ErrorOr<T> result, string? message = null)
        => BaseControllerHelpers.HandleResult(result, message);

    // Object thường
    public static IActionResult HandleResult<T>(T value, string? message = null)
        => BaseControllerHelpers.HandleResult(value, 200, message);
}

public static class CREATE
{
    // ErrorOr<T>
    public static IActionResult HandleResult<T>(ErrorOr<T> result, string? message = null)
    {
        if (!result.IsError)
            return BaseControllerHelpers.HandleResult(result.Value, 201, message);

        return BaseControllerHelpers.HandleResult(result, message);
    }

    // Object thường
    public static IActionResult HandleResult<T>(T value, string? message = null)
        => BaseControllerHelpers.HandleResult(value, 201, message);
}

public static class NO_CONTENT
{
    // ErrorOr<T>
    public static IActionResult HandleResult<T>(ErrorOr<T> result)
    {
        if (!result.IsError)
            return BaseControllerHelpers.HandleResult(result.Value, 204);

        return BaseControllerHelpers.HandleResult(result);
    }

    // Object thường
    public static IActionResult HandleResult<T>(T value)
        => BaseControllerHelpers.HandleResult(value, 204);
}
