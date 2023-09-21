namespace RadzenBook.Infrastructure.Middlewares;

public class AppExceptionResponse
{
    public AppExceptionResponse(int statusCode, string title, string? details = "", string? type = "", string? traceId = "", string? innerException = "")
    {
        StatusCode = statusCode;
        Title = title;
        Details = details;
        Type = type;
        TraceId = traceId;
        InnerException = innerException;
    }
    
    public string? TraceId { get; set; }
    public string? Type { get; set; }
    public int StatusCode { get; set; }
    public string Title { get; set; }
    public string? Details { get; set; }
    public string? InnerException { get; set; }
}