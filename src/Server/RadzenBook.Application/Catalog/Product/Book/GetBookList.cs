namespace RadzenBook.Application.Catalog.Product.Book;

public class GetBookListRequest : GetProductListRequest<BookDto, BookPagingParams>
{
}

public class GetBookListHandler : GetProductListHandler<GetBookListRequest, BookDto, BookPagingParams>
{
    public GetBookListHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t)
        : base(unitOfWork, mapper, loggerFactory, t)
    {
    }
}