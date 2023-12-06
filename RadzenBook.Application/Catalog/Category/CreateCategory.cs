using RadzenBook.Application.Common.Security;

namespace RadzenBook.Application.Catalog.Category;

public class CreateCategoryRequest : IRequest<Result<Unit>>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}

public class CreateCategoryRequestValidator : CustomValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator(IStringLocalizer<CreateCategoryRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(t["Title is required"])
            .MaximumLength(50).WithMessage(t["Title must not exceed {0} characters", 50]);
        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage(t["Description must not exceed {0} characters", 2000]);
    }
}

public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<CreateCategoryRequestHandler> _logger;

    public CreateCategoryRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<CreateCategoryRequestHandler>();
        _t = t.Create(typeof(CreateCategoryRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var category = _mapper.Map<Domain.Catalog.Category>(request);
            category.CreatedBy = _userAccessor.GetUsername();
            category.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<ICategoryRepository, Domain.Catalog.Category, Guid>()
                .CreateAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Category created successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(CreateCategoryRequestHandler), e.Message, e);
        }
    }
}