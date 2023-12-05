namespace RadzenBook.Application.Catalog.Author;

public class GetAuthorByIdRequest : IRequest<Result<AuthorDto>>
{
    public Guid Id { get; set; }
}

public class GetAuthorByIdRequestHandler : IRequestHandler<GetAuthorByIdRequest, Result<AuthorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAuthorByIdRequestHandler> _logger;
    private readonly IStringLocalizer _t;

    public GetAuthorByIdRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory logger,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetAuthorByIdRequestHandler>();
        _t = t.Create(typeof(GetAuthorByIdRequestHandler));
    }

    public async Task<Result<AuthorDto>> Handle(GetAuthorByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var author = await _unitOfWork.GetRepository<IAuthorRepository, Domain.Catalog.Author, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (author is null)
            {
                return Result<AuthorDto>.Failure(_t["Author not found."]);
            }

            var authorDto = _mapper.Map<AuthorDto>(author);
            return Result<AuthorDto>.Success(authorDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetAuthorByIdRequestHandler), e.Message, e);
        }
    }
}