namespace TicketHive.Api.Common;
public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; } = 200;
    public object? Meta { get; set; } 

    public string ErrorCode { get; set; } = string.Empty;
}
