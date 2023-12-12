using RadzenBook.Application.Common.Photo;

namespace RadzenBook.Application.Catalog.Author;

public class CreateAuthorRequest : IRequest<Result<Unit>>
{
    public string? FullName { get; set; } = default!;
    public string? Alias { get; set; } = default!;
    public string? Biography { get; set; } = default!;
    public IFormFile? Image { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; } = default!;
    public DateTime? DateOfDeath { get; set; } = default!;
}

public class CreateAuthorRequestValidator : CustomValidator<CreateAuthorRequest>
{
    public CreateAuthorRequestValidator(IStringLocalizer<CreateAuthorRequestValidator> t) : base(t)
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(t["FullName is required"])
            .MaximumLength(50).WithMessage(t["FullName must not exceed {0} characters", 450]);
        RuleFor(x => x.Alias)
            .MaximumLength(450).WithMessage(t["Alias must not exceed {0} characters", 450]);
        RuleFor(x => x.Biography)
            .MaximumLength(2000).WithMessage(t["Biography must not exceed {0} characters", 2000]);
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage(t["DateOfBirth must be less than today"]);
        RuleFor(x => x.DateOfDeath)
            .GreaterThan(x => x.DateOfBirth).WithMessage(t["DateOfDeath must be greater than DateOfBirth"]);
    }
}

public class CreateAuthorRequestHandler : IRequestHandler<CreateAuthorRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly IPhotoAccessor _photoAccessor;
    private readonly ILogger<CreateAuthorRequestHandler> _logger;


    public CreateAuthorRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<CreateAuthorRequestHandler>();
        _t = t.Create(typeof(CreateAuthorRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
        _photoAccessor = infrastructureServiceManager.PhotoAccessor;
    }

    public async Task<Result<Unit>> Handle(CreateAuthorRequest request, CancellationToken cancellationToken = default!)
    {
        try
        {
            var author = _mapper.Map<Domain.Catalog.Author>(request);
            author.CreatedBy = _userAccessor.GetUsername();
            author.ModifiedBy = _userAccessor.GetUsername();
            if (request.Image != null)
            {
                var photoUploadResult = await _photoAccessor.AddPhotoAsync(request.Image);
                author.ImageUrl = photoUploadResult.Url;
            }

            await _unitOfWork.GetRepository<IAuthorRepository, Domain.Catalog.Author, Guid>()
                .CreateAsync(author, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Create author successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(CreateAuthorRequestHandler), e.Message, e);
        }
    }
}