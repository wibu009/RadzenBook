namespace RadzenBook.Application.Catalog.Genre;

public class GetGenreListRequest : IRequest<Result<PaginatedList<GenreDto>>>
{
    public GenrePagingParams PagingParams { get; set; }
}

public class GetGenreListRequestHandler : IRequestHandler<GetGenreListRequest, Result<PaginatedList<GenreDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetGenreListRequestHandler> _logger;

    public GetGenreListRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<GetGenreListRequestHandler>();
    }

    public async Task<Result<PaginatedList<GenreDto>>> Handle(GetGenreListRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var genres = await _unitOfWork.GetRepository<IGenreRepository, Domain.Catalog.Genre, Guid>()
                .GetPagedAsync(
                    filter: x =>
                        string.IsNullOrEmpty(request.PagingParams.Name) ||
                        x.Name!.Contains(request.PagingParams.Name),
                    pageNumber: request.PagingParams.PageNumber,
                    pageSize: request.PagingParams.PageSize,
                    includeProperties: "Books",
                    cancellationToken: cancellationToken);
            var totalCount = await _unitOfWork.GetRepository<IGenreRepository, Domain.Catalog.Genre, Guid>()
                .CountAsync(
                    filter: x =>
                        string.IsNullOrEmpty(request.PagingParams.Name) ||
                        x.Name!.Contains(request.PagingParams.Name),
                    cancellationToken: cancellationToken);

            var genresDto = _mapper.Map<List<GenreDto>>(genres);
            var genresDtoPaginated = new PaginatedList<GenreDto>(genresDto, totalCount,
                request.PagingParams.PageNumber,
                request.PagingParams.PageSize);
            return Result<PaginatedList<GenreDto>>.Success(genresDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetGenreListRequestHandler), e.Message, e);
        }
    }
}