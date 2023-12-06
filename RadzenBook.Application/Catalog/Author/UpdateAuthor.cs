using RadzenBook.Application.Common.Photo;
using RadzenBook.Application.Common.Security;

namespace RadzenBook.Application.Catalog.Author;

public class UpdateAuthorRequest : IRequest<Result<Unit>>
{
    [JsonIgnore] public Guid Id { get; set; }
    public string? FullName { get; set; } = default!;
    public string? Alias { get; set; } = default!;
    public string? Biography { get; set; } = default!;
    public IFormFile? Image { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }
}

public class UpdateAuthorRequestValidator : CustomValidator<UpdateAuthorRequest>
{
    public UpdateAuthorRequestValidator(IStringLocalizer<UpdateAuthorRequestValidator> t) : base(t)
    {
        RuleFor(x => x.FullName)
            .MaximumLength(50).WithMessage(t["FullName must not exceed {0} characters", 450]);
        RuleFor(x => x.Alias)
            .MaximumLength(450).WithMessage(t["Alias must not exceed {0} characters", 450]);
        RuleFor(x => x.Biography)
            .MaximumLength(2000).WithMessage(t["Biography must not exceed {0} characters", 2000]);
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).When(x => x.DateOfBirth.HasValue)
            .WithMessage(t["DateOfBirth must be less than today"]);
        RuleFor(x => x.DateOfDeath)
            .GreaterThan(x => x.DateOfBirth).When(x => x.DateOfBirth.HasValue && x.DateOfDeath.HasValue)
            .WithMessage(t["DateOfDeath must be greater than DateOfBirth"]);
    }
}

public class UpdateAuthorRequestHandler : IRequestHandler<UpdateAuthorRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly IPhotoAccessor _photoAccessor;
    private readonly ILogger<UpdateAuthorRequestHandler> _logger;

    public UpdateAuthorRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<UpdateAuthorRequestHandler>();
        _t = t.Create(typeof(UpdateAuthorRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
        _photoAccessor = infrastructureServiceManager.PhotoAccessor;
    }

    public async Task<Result<Unit>> Handle(UpdateAuthorRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var author = await _unitOfWork.GetRepository<IAuthorRepository, Domain.Catalog.Author, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (author == null)
            {
                return Result<Unit>.Failure(_t["Author not found"]);
            }

            _mapper.Map(request, author);
            author.ModifiedBy = _userAccessor.GetUsername();
            if (request.Image is not null)
            {
                var photoUploadResult = !string.IsNullOrEmpty(author.ImageUrl)
                    ? await _photoAccessor.UpdatePhotoAsync(request.Image, author.ImageUrl)
                    : await _photoAccessor.AddPhotoAsync(request.Image);

                author.ImageUrl = photoUploadResult.Url;
            }

            await _unitOfWork.GetRepository<IAuthorRepository, Domain.Catalog.Author, Guid>()
                .UpdateAsync(author, cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Update author successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(UpdateAuthorRequestHandler), e.Message, e);
        }
    }
}