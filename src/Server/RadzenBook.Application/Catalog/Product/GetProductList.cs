namespace RadzenBook.Application.Catalog.Product;

public class GetProductListRequest<TProductDto, TPagingParams> : IRequest<Result<PaginatedList<TProductDto>>>
    where TProductDto : ProductDto
    where TPagingParams : ProductPagingParams
{
    public TPagingParams PagingParams { get; set; } = default!;
}

public class GetProductListHandler<TGetProductListRequest, TProductDto, TPagingParams> : IRequestHandler<
    GetProductListRequest<TProductDto, TPagingParams>, Result<PaginatedList<TProductDto>>>
    where TGetProductListRequest : GetProductListRequest<TProductDto, TPagingParams>
    where TProductDto : ProductDto
    where TPagingParams : ProductPagingParams
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IMapper Mapper;
    protected readonly ILogger<GetProductListHandler<TGetProductListRequest, TProductDto, TPagingParams>> Logger;
    protected readonly IStringLocalizer T;

    public GetProductListHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
        Logger = loggerFactory
            .CreateLogger<GetProductListHandler<TGetProductListRequest, TProductDto, TPagingParams>>();
        T = t.Create(typeof(GetProductListHandler<TGetProductListRequest, TProductDto, TPagingParams>));
    }

    public virtual async Task<Result<PaginatedList<TProductDto>>> Handle(
        GetProductListRequest<TProductDto, TPagingParams> request, CancellationToken cancellationToken)
    {
        // override this method in derived class
        return await Task.FromResult(
            Result<PaginatedList<TProductDto>>.Success(new PaginatedList<TProductDto>(new List<TProductDto>(), 0, 0,
                0)));
    }
}