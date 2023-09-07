namespace RadzenBook.Contract.Core;

public class PaginatedList<T> : List<T> where T : class
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public PaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(
        IEnumerable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return await Task.FromResult(new PaginatedList<T>(items, count, pageNumber, pageSize));
    }
}