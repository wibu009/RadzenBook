namespace RadzenBook.Application.Catalog.Product;

public class GetProductByIdRequest<TProductDto> : IRequest<Result<TProductDto>> where TProductDto : ProductDto
{
    public Guid Id { get; set; }
}

public class
    GetProductByIdHandler<TGetProductByIdRequest, TProductDto> : IRequestHandler<TGetProductByIdRequest,
    Result<TProductDto>>
    where TGetProductByIdRequest : GetProductByIdRequest<TProductDto>
    where TProductDto : ProductDto, new()
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IMapper Mapper;
    protected readonly ILogger<GetProductByIdHandler<TGetProductByIdRequest, TProductDto>> Logger;
    protected readonly IStringLocalizer T;

    public GetProductByIdHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
        Logger = loggerFactory.CreateLogger<GetProductByIdHandler<TGetProductByIdRequest, TProductDto>>();
        T = t.Create(typeof(GetProductByIdHandler<TGetProductByIdRequest, TProductDto>));
    }

    public async Task<Result<TProductDto>> Handle(TGetProductByIdRequest request, CancellationToken cancellationToken)
    {
        // override this method in derived class
        return await Task.FromResult(Result<TProductDto>.Success(new TProductDto()));
    }
}