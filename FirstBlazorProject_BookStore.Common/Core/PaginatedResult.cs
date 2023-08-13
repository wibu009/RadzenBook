namespace FirstBlazorProject_BookStore.Common.Core;

public class PaginatedResult<TEntity> where TEntity : class
{
    public IEnumerable<TEntity> Items { get; set; } = new List<TEntity>();
    public int Count { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(Count / (double)PageSize);
}