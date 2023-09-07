﻿namespace FirstBlazorProject_BookStore.Model.Core;

public class Result<T>
{
    public T? Value { get; set; }
    public bool IsSuccess { get; set; }
    public int? StatusCode { get; set; }
    public string? Error { get; set; }

    public static Result<T> Success(T value) => new Result<T> { Value = value, IsSuccess = true, StatusCode = Constants.StatusCode.Success };
    public static Result<T> Success(T value, int statusCode) => new Result<T> { Value = value, IsSuccess = true, StatusCode = statusCode };
    public static Result<T> Failure(string error) => new Result<T> { Error = error, IsSuccess = false, StatusCode = Constants.StatusCode.InternalServerError };
    public static Result<T> Failure(int statusCode, string error) => new Result<T> { Error = error, IsSuccess = false, StatusCode = statusCode };
}