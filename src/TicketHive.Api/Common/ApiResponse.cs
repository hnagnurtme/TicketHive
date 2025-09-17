namespace TicketHive.Api.Common;
public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; } = 200;
    public object? Meta { get; set; } 

    public string ErrorCode { get; set; } = string.Empty;
    // 200 OK
    public static ApiResponse<T> Ok(T data, string? message = null, object? meta = null)
        => new ApiResponse<T> { Success = true, Data = data, Message = message, StatusCode = 200, Meta = meta };

    // 201 Created
    public static ApiResponse<T> Created(T data, string? message = null, object? meta = null)
        => new ApiResponse<T> { Success = true, Data = data, Message = message, StatusCode = 201, Meta = meta };

    // 204 No Content
    public static ApiResponse<T> NoContent(string? message = null, object? meta = null)
        => new ApiResponse<T> { Success = true, Data = default, Message = message, StatusCode = 204, Meta = meta };

    // 400 Bad Request
    public static ApiResponse<T> BadRequest(string message, object? meta = null)
        => new ApiResponse<T> { Success = false, Data = default, Message = message, StatusCode = 400, Meta = meta };

    // 404 Not Found
    public static ApiResponse<T> NotFound(string message, object? meta = null)
        => new ApiResponse<T> { Success = false, Data = default, Message = message, StatusCode = 404, Meta = meta };

    // 500 Internal Server Error
    public static ApiResponse<T> ServerError(string message, object? meta = null)
        => new ApiResponse<T> { Success = false, Data = default, Message = message, StatusCode = 500, Meta = meta };
}
