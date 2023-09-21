namespace RadzenBook.Application.Common.Models;

public class Result<T>
{
    public bool IsSuccess { get; set; } = default!;
    public string? Message { get; set; } = default!;
    public T? Value { get; set; } = default!;
    public int StatusCode { get; set; } = default!;
    public static Result<T> Success() => new() { IsSuccess = true, StatusCode = 200 };
    public static Result<T> Success(string message) => new() { IsSuccess = true, Message = message, StatusCode = 200 };
    public static Result<T> Success(T data) => new() { IsSuccess = true, Value = data, StatusCode = 200 };
    public static Result<T> Failure(string message, int statusCode) => new() { IsSuccess = false, Message = message, StatusCode = statusCode };
    public static Result<T> Failure(string message) => new() { IsSuccess = false, Message = message, StatusCode = 400 };
}