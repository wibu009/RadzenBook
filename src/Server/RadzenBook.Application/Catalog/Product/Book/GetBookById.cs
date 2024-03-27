namespace RadzenBook.Application.Catalog.Product.Book;

public class GetBookByIdRequest : GetProductByIdRequest<BookDto>
{
}

public class GetBookByIdHandler : GetProductByIdHandler<GetBookByIdRequest, BookDto>
{
    public GetBookByIdHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t)
        : base(unitOfWork, mapper, loggerFactory, t)
    {
    }
}