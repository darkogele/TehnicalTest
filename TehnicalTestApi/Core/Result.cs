namespace TechnicalTestApi.Core;

public class Result<T>
{
    public bool IsSuccess { get; private init; }
    public T? Data { get; private init; }
    public string? Error { get; private init; }
    public string? Message { get; private init; }

    public static Result<T> Success(T data, string? message = null) => new()
    {
        IsSuccess = true,
        Data = data,
        Message = message
    };

    public static Result<T> Failure(string error) => new()
    {
        IsSuccess = false,
        Error = error
    };
}