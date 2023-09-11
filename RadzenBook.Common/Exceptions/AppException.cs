namespace RadzenBook.Common.Exceptions;

public class AppException
{
    public AppException(int statusCode, string title, string? details = "", string? type = "", string? traceId = "")
    {
        StatusCode = statusCode;
        Title = title;
        Details = details;
        Type = type;
        TraceId = traceId;
    }
    
    public string? TraceId { get; set; }
    public string? Type { get; set; }
    public int StatusCode { get; set; }
    public string Title { get; set; }
    public string? Details { get; set; }
}