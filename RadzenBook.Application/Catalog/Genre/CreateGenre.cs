using RadzenBook.Application.Catalog.Category;

namespace RadzenBook.Application.Catalog.Genre;

public class CreateGenreRequest : IRequest<Result<Unit>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class CreateGenreRequestValidator : CustomValidator<CreateGenreRequest>
{
    public CreateGenreRequestValidator(IStringLocalizer<CreateGenreRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(t["Name is required"])
            .MaximumLength(50).WithMessage(t["Name must not exceed {0} characters", 50]);
        RuleFor(x => x.Description)
            .MaximumLength(2500).WithMessage(t["Description must not exceed {0} characters", 2500]);
    }
}

public class CreateGenreRequestHandler : IRequestHandler<CreateGenreRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<CreateCategoryRequestHandler> _logger;

    public CreateGenreRequestHandler(
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

    public async Task<Result<Unit>> Handle(CreateGenreRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var genre = _mapper.Map<Domain.Catalog.Genre>(request);
            genre.CreatedBy = _userAccessor.GetUsername();
            genre.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IGenreRepository, Domain.Catalog.Genre, Guid>()
                .CreateAsync(genre, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Genre created successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(CreateGenreRequestHandler), e.Message, e);
        }
    }
}