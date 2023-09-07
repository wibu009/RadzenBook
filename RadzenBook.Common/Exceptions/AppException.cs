namespace FirstBlazorProject_BookStore.Common.Exceptions;

public class AppException
{
    public AppException(int statusCode, string message, string? details = "", string stackTrace = "")
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
        StackTrace = stackTrace;
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string? Details { get; set; }
    public string? StackTrace { get; set; }
}