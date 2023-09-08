namespace RadzenBook.Contract.Core;

public class Result<T> where T : class
{
    public bool IsSuccess { get; set; } = default!;
    public string Error { get; set; } = default!;
    public T Value { get; set; } = default!;

    public static Result<T> Success(T data) => new() { IsSuccess = true, Value = data };
    public static Result<T> Failure(string message) => new() { IsSuccess = false, Error = message };
}