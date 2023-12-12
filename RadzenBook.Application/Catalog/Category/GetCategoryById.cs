namespace RadzenBook.Application.Catalog.Category;

public class GetCategoryByIdRequest : IRequest<Result<CategoryDto>>
{
    public Guid Id { get; set; }
}

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdRequest, Result<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCategoryByIdHandler> _logger;
    private readonly IStringLocalizer _t;

    public GetCategoryByIdHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<GetCategoryByIdHandler>();
        _t = t.Create(typeof(GetCategoryByIdHandler));
    }

    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var category = await _unitOfWork.GetRepository<ICategoryRepository, Domain.Catalog.Category, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken, includeProperties: "Products");
            if (category == null)
                return Result<CategoryDto>.Failure(_t["Category not found"]);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Result<CategoryDto>.Success(categoryDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result<CategoryDto>.Failure(e.Message);
        }
    }
}