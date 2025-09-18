namespace TicketHive.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public string? ErrorCode { get; }
    public string Message { get; }
    public T Value { get; }

    private Result(bool isSuccess, T value, string? errorCode, string message)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorCode = errorCode;
        Message = message;
    }

    public static Result<T> Success(T value, string message = "Success")
        => new Result<T>(true, value, null, message);

    public static Result<T> Failure(string errorCode, string message)
        => new Result<T>(false, default(T)!, errorCode, message);
}
