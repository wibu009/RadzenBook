namespace RadzenBook.Application.Catalog.Genre;

public class GetGenreByIdRequest : IRequest<Result<GenreDto>>
{
    public Guid Id { get; set; }
}

public class GetGenreByIdHandler : IRequestHandler<GetGenreByIdRequest, Result<GenreDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetGenreByIdHandler> _logger;
    private readonly IStringLocalizer _t;

    public GetGenreByIdHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<GetGenreByIdHandler>();
        _t = t.Create(typeof(GetGenreByIdHandler));
    }

    public async Task<Result<GenreDto>> Handle(GetGenreByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var genre = await _unitOfWork.GetRepository<IGenreRepository, Domain.Catalog.Genre, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken, includeProperties: "Books");
            if (genre == null)
                return Result<GenreDto>.Failure(_t["Genre not found"]);
            var genreDto = _mapper.Map<GenreDto>(genre);
            return Result<GenreDto>.Success(genreDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetGenreByIdHandler), e.Message, e);
        }
    }
}