namespace RadzenBook.Application.Catalog.Demo;

public class GetDemoListRequest : IRequest<Result<PaginatedList<DemoDto>>>
{
    public PagingParams PagingParams { get; set; } = new();
}

public class GetDemoListRequestHandler : IRequestHandler<GetDemoListRequest, Result<PaginatedList<DemoDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDemoListRequestHandler> _logger;
    private readonly IStringLocalizer _t;

    public GetDemoListRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory logger,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetDemoListRequestHandler>();
        _t = t.Create(typeof(GetDemoListRequestHandler));
    }

    public async Task<Result<PaginatedList<DemoDto>>> Handle(GetDemoListRequest listRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var demos = await _unitOfWork.GetRepository<IDemoRepository, Domain.Catalog.Demo, Guid>()
                .GetPagedAsync(pageNumber: listRequest.PagingParams.PageNumber,
                    pageSize: listRequest.PagingParams.PageSize,
                    cancellationToken: cancellationToken);
            var totalCount = await _unitOfWork.GetRepository<IDemoRepository, Domain.Catalog.Demo, Guid>()
                .CountAsync(cancellationToken: cancellationToken);
            var demosDto = _mapper.Map<List<DemoDto>>(demos);
            var demosDtoPaginated = new PaginatedList<DemoDto>(demosDto, totalCount,
                listRequest.PagingParams.PageNumber,
                listRequest.PagingParams.PageSize);
            return Result<PaginatedList<DemoDto>>.Success(demosDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetDemoListRequestHandler), e.Message, e);
        }
    }
}