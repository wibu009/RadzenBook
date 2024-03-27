namespace RadzenBook.Application.Catalog.Author;

public class GetAuthorListRequest : IRequest<Result<PaginatedList<AuthorDto>>>
{
    public AuthorPagingParams PagingParams { get; set; } = default!;
}

public class GetAuthorListRequestHandler : IRequestHandler<GetAuthorListRequest, Result<PaginatedList<AuthorDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAuthorListRequestHandler> _logger;

    public GetAuthorListRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<GetAuthorListRequestHandler>();
        t.Create(typeof(GetAuthorListRequestHandler));
    }

    public async Task<Result<PaginatedList<AuthorDto>>> Handle(GetAuthorListRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Expression<Func<Domain.Catalog.Author, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.PagingParams.Name) ||
                !string.IsNullOrEmpty(request.PagingParams.Alias))
            {
                filter = author =>
                    (string.IsNullOrEmpty(request.PagingParams.Name) ||
                     author.FullName!.Contains(request.PagingParams.Name) &&
                     string.IsNullOrEmpty(request.PagingParams.Alias) ||
                     author.Alias!.Contains(request.PagingParams.Alias!));
            }

            string? orderBy = null;
            if (!string.IsNullOrWhiteSpace(request.PagingParams.OrderBy))
            {
                orderBy = request.PagingParams.OrderBy.ToLower() switch
                {
                    "name" => nameof(Domain.Catalog.Author.FullName),
                    "alias" => nameof(Domain.Catalog.Author.Alias),
                    _ => nameof(Domain.Catalog.Author.FullName)
                };
            }

            var authors = await _unitOfWork.GetRepository<IAuthorRepository, Domain.Catalog.Author, Guid>()
                .GetPagedAsync(
                    filter: filter,
                    pageNumber: request.PagingParams.PageNumber,
                    pageSize: request.PagingParams.PageSize,
                    orderBy: orderBy,
                    isAscending: request.PagingParams.IsAscending,
                    cancellationToken: cancellationToken);
            var totalCount = await _unitOfWork.GetRepository<IAuthorRepository, Domain.Catalog.Author, Guid>()
                .CountAsync(filter: filter, cancellationToken: cancellationToken);

            var authorsDto = _mapper.Map<List<AuthorDto>>(authors);
            var authorsDtoPaginated = new PaginatedList<AuthorDto>(authorsDto, totalCount,
                request.PagingParams.PageNumber,
                request.PagingParams.PageSize);

            return Result<PaginatedList<AuthorDto>>.Success(authorsDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetAuthorListRequestHandler), e.Message, e);
        }
    }
}