namespace RadzenBook.Application.Catalog.Product.Book;

public class BookPagingParams : ProductPagingParams
{
    public List<string> Genres { get; set; } = new();
    public string Author { get; set; } = default!;
    public string Publisher { get; set; } = default!;
    public string Language { get; set; } = default!;
}