namespace RadzenBook.Application.Catalog.Publisher;

public class GetPublisherListRequest : IRequest<Result<PaginatedList<PublisherDto>>>
{
    public PagingParams PagingParams { get; set; } = new();
}
public class GetPublisherListRequestHandler : IRequestHandler<GetPublisherListRequest, Result<PaginatedList<PublisherDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPublisherListRequestHandler> _logger;

    public GetPublisherListRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<GetPublisherListRequestHandler>();
    }

    public async Task<Result<PaginatedList<PublisherDto>>> Handle(GetPublisherListRequest listRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var publishers = await _unitOfWork.GetRepository<IPublisherRepository, Domain.Catalog.Publisher, Guid>()
                .GetPagedAsync(pageNumber: listRequest.PagingParams.PageNumber,
                    pageSize: listRequest.PagingParams.PageSize,
                    cancellationToken: cancellationToken);
            var totalCount = await _unitOfWork.GetRepository<IPublisherRepository, Domain.Catalog.Publisher, Guid>()
                .CountAsync(cancellationToken: cancellationToken);
            var publishersDto = _mapper.Map<List<PublisherDto>>(publishers);
            var publishersDtoPaginated = new PaginatedList<PublisherDto>(publishersDto, totalCount,
                listRequest.PagingParams.PageNumber,
                listRequest.PagingParams.PageSize);
            return Result<PaginatedList<PublisherDto>>.Success(publishersDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetPublisherListRequestHandler), e.Message, e);
        }
    }
}