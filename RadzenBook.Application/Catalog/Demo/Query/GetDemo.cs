using RadzenBook.Application.Common.Exceptions;

namespace RadzenBook.Application.Catalog.Demo.Query;

public class GetDemoRequest : IRequest<Result<PaginatedList<DemoDto>>>
{
    public PagingParams PagingParams { get; set; } = new();
}

public class GetPagedDemoRequestHandler : IRequestHandler<GetDemoRequest, Result<PaginatedList<DemoDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPagedDemoRequestHandler> _logger;
    
    public GetPagedDemoRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, ILoggerFactory logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetPagedDemoRequestHandler>();
    }

    public async Task<Result<PaginatedList<DemoDto>>> Handle(GetDemoRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var demos = await _unitOfWork.GetRepository<IDemoRepository, Domain.Catalog.Demo, Guid>().GetPagedAsync(pageNumber: request.PagingParams.PageNumber, pageSize: request.PagingParams.PageSize, cancellationToken: cancellationToken);
            var totalCount = await _unitOfWork.GetRepository<IDemoRepository, Domain.Catalog.Demo, Guid>().CountAsync(cancellationToken: cancellationToken);
            var demosDto = _mapper.Map<List<DemoDto>>(demos);
            var demosDtoPaginated = new PaginatedList<DemoDto>(demosDto, totalCount, request.PagingParams.PageNumber, request.PagingParams.PageSize);
            return Result<PaginatedList<DemoDto>>.Success(demosDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(Handle), nameof(GetPagedDemoRequestHandler), e.Message, e);
        }
    }
}