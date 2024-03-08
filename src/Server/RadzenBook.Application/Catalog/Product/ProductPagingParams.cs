namespace RadzenBook.Application.Catalog.Product;

public class ProductPagingParams : PagingParams
{
    public string Title { get; set; } = default!;
    public string Category { get; set; } = default!;
    public decimal MinPrice { get; set; } = 0;
    public decimal MaxPrice { get; set; } = 0;
    public string SortBy { get; set; } = default!;
}