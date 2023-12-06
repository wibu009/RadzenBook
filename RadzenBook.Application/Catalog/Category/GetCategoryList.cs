namespace RadzenBook.Application.Catalog.Category;

public class GetCategoryListRequest : IRequest<Result<PaginatedList<CategoryDto>>>
{
    public CategoryPagingParams PagingParams { get; set; }
}

public class GetCategoryListRequestHandler : IRequestHandler<GetCategoryListRequest, Result<PaginatedList<CategoryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCategoryListRequestHandler> _logger;

    public GetCategoryListRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<GetCategoryListRequestHandler>();
    }

    public async Task<Result<PaginatedList<CategoryDto>>> Handle(GetCategoryListRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var categories = await _unitOfWork.GetRepository<ICategoryRepository, Domain.Catalog.Category, Guid>()
                .GetPagedAsync(
                    filter: x =>
                        string.IsNullOrEmpty(request.PagingParams.Title) ||
                        x.Title!.Contains(request.PagingParams.Title),
                    pageNumber: request.PagingParams.PageNumber,
                    pageSize: request.PagingParams.PageSize,
                    cancellationToken: cancellationToken);
            var totalCount = await _unitOfWork.GetRepository<ICategoryRepository, Domain.Catalog.Category, Guid>()
                .CountAsync(
                    filter: x =>
                        string.IsNullOrEmpty(request.PagingParams.Title) ||
                        x.Title!.Contains(request.PagingParams.Title),
                    cancellationToken: cancellationToken);

            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
            foreach (var categoryDto in categoriesDto)
            {
                categoryDto.TotalProducts = await _unitOfWork
                    .GetRepository<IProductRepository, Product, Guid>()
                    .CountAsync(filter: x => x.CategoryId == categoryDto.Id, cancellationToken: cancellationToken);
            }

            var categoriesDtoPaginated = new PaginatedList<CategoryDto>(categoriesDto, totalCount,
                request.PagingParams.PageNumber,
                request.PagingParams.PageSize);

            return Result<PaginatedList<CategoryDto>>.Success(categoriesDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetCategoryListRequestHandler), e.Message, e);
        }
    }
}