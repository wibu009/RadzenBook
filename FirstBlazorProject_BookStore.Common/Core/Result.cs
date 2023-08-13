namespace FirstBlazorProject_BookStore.Common.Core;

public class Result<T>
{
    public T? Value { get; set; }
    public bool IsSuccess { get; set; }
    public int? StatusCode { get; set; }
    public string? Error { get; set; }

    public static Result<T> Success(T value) => new Result<T> { Value = value, IsSuccess = true, StatusCode = 200 };
    public static Result<T> Success(T value, int statusCode) => new Result<T> { Value = value, IsSuccess = true, StatusCode = statusCode };
    public static Result<T> Failure(string error) => new Result<T> { Error = error, IsSuccess = false, StatusCode = 500 };
    public static Result<T> Failure(int statusCode, string error) => new Result<T> { Error = error, IsSuccess = false, StatusCode = statusCode };
}