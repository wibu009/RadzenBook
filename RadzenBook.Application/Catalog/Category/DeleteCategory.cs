namespace RadzenBook.Application.Catalog.Category;

public class DeleteCategoryRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<DeleteCategoryRequestHandler> _logger;

    public DeleteCategoryRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<DeleteCategoryRequestHandler>();
        _t = t.Create(typeof(DeleteCategoryRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _unitOfWork.GetRepository<ICategoryRepository, Domain.Catalog.Category, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (category == null)
                return Result<Unit>.Failure(_t["Category not found"]);
            category.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<ICategoryRepository, Domain.Catalog.Category, Guid>()
                .SoftDeleteAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Category deleted successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(DeleteCategoryRequestHandler), e.Message, e);
        }
    }
}