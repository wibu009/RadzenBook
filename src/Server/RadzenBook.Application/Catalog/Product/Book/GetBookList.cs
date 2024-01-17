namespace RadzenBook.Application.Catalog.Product.Book;

public class GetBookListRequest : IRequest<Result<PaginatedList<BookDto>>>
{
    public BookPagingParams PagingParams { get; set; } = default!;
}