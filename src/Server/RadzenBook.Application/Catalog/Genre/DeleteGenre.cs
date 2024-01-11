namespace RadzenBook.Application.Catalog.Genre;

public class DeleteGenreRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteGenreRequestValidator : CustomValidator<DeleteGenreRequest>
{
    public DeleteGenreRequestValidator(IStringLocalizer<DeleteGenreRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(t["Id is required"]);
    }
}

public class DeleteGenreRequestHandler : IRequestHandler<DeleteGenreRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<DeleteGenreRequestHandler> _logger;

    public DeleteGenreRequestHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _logger = loggerFactory.CreateLogger<DeleteGenreRequestHandler>();
        _t = t.Create(typeof(DeleteGenreRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(DeleteGenreRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var genre = await _unitOfWork.GetRepository<IGenreRepository, Domain.Catalog.Genre, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (genre == null)
                return Result<Unit>.Failure(_t["Genre not found"]);
            genre.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IGenreRepository, Domain.Catalog.Genre, Guid>()
                .SoftDeleteAsync(genre, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Genre deleted successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(DeleteGenreRequestHandler), e.Message, e);
        }
    }
}