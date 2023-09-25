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
}