using System.Text.Json;

namespace RadzenBook.Infrastructure.Common.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
        var paginationHeader = new {
            currentPage,
            itemsPerPage,
            totalItems,
            totalPages
        };

        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
    }
    
    public static string GetIpAddress(this HttpContext context)
    {
        var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = context.Connection.RemoteIpAddress?.ToString();
        }
        return ip ?? string.Empty;
    }
    
    public static string GetUrl(this HttpRequest request)
    {
        return request.Headers.TryGetValue("Referer", out var header) ? header.ToString() : string.Empty;
    }
}