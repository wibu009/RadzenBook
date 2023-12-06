using RadzenBook.Application.Common.Security;

namespace RadzenBook.Application.Catalog.Category;

public class UpdateCategoryRequest : IRequest<Result<Unit>>
{
    [JsonIgnore] public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}

public class UpdateCategoryRequestValidator : CustomValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator(IStringLocalizer<UpdateCategoryRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Title)
            .MaximumLength(50).WithMessage(t["Title must not exceed {0} characters", 50]);
        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage(t["Description must not exceed {0} characters", 2000]);
    }
}

public class UpdateCategoryRequestHandler : IRequestHandler<UpdateCategoryRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<UpdateCategoryRequestHandler> _logger;

    public UpdateCategoryRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<UpdateCategoryRequestHandler>();
        _t = t.Create(typeof(UpdateCategoryRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _unitOfWork.GetRepository<ICategoryRepository, Domain.Catalog.Category, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (category == null)
                return Result<Unit>.Failure(_t["Category not found"]);
            _mapper.Map(request, category);
            category.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Category updated successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(UpdateCategoryRequestHandler), e.Message, e);
        }
    }
}