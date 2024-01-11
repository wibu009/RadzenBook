namespace RadzenBook.Application.Catalog.Genre;

public class UpdateGenreRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class UpdateGenreRequestValidator : CustomValidator<UpdateGenreRequest>
{
    public UpdateGenreRequestValidator(IStringLocalizer<UpdateGenreRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(t["Name is required"])
            .MaximumLength(50).WithMessage(t["Name must not exceed {0} characters", 50]);
        RuleFor(x => x.Description)
            .MaximumLength(2500).WithMessage(t["Description must not exceed {0} characters", 2500]);
    }
}

public class UpdateGenreRequestHandler : IRequestHandler<UpdateGenreRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<UpdateGenreRequestHandler> _logger;

    public UpdateGenreRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<UpdateGenreRequestHandler>();
        _t = t.Create(typeof(UpdateGenreRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(UpdateGenreRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var genre = await _unitOfWork.GetRepository<IGenreRepository, Domain.Catalog.Genre, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (genre == null)
                return Result<Unit>.Failure(_t["Genre not found"]);
            genre.ModifiedBy = _userAccessor.GetUsername();
            _mapper.Map(request, genre);
            genre.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Genre updated successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(UpdateGenreRequestHandler), e.Message, e);
        }
    }
}