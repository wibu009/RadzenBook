namespace RadzenBook.Application.Catalog.Product.Book;

public class BookPagingParams : PagingParams
{
    public string Title { get; set; } = default!;
    public List<string> Genres { get; set; } = new();
    public string Author { get; set; } = default!;
    public string Publisher { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string Language { get; set; } = default!;
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
}