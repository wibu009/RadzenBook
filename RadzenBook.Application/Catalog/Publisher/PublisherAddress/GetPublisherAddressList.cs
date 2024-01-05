namespace RadzenBook.Application.Catalog.PublisherAddress;

public class GetPublisherAddressListRequest : IRequest<Result<PaginatedList<PublisherAddressDto>>>
{
    public PagingParams PagingParams { get; set; } = new();
}
public class GetPublisherAddressListRequestHandler : IRequestHandler<GetPublisherAddressListRequest, Result<PaginatedList<PublisherAddressDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPublisherAddressListRequestHandler> _logger;

    public GetPublisherAddressListRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<GetPublisherAddressListRequestHandler>();
    }
    public async Task<Result<PaginatedList<PublisherAddressDto>>> Handle(GetPublisherAddressListRequest listRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var publisherAddres = await _unitOfWork.GetRepository<IPublisherAddressRepository, Domain.Catalog.PublisherAddress, Guid>()
                .GetPagedAsync(pageNumber: listRequest.PagingParams.PageNumber,
                    pageSize: listRequest.PagingParams.PageSize,
                    cancellationToken: cancellationToken);
            var totalCount = await _unitOfWork.GetRepository<IPublisherAddressRepository, Domain.Catalog.PublisherAddress, Guid>()
                .CountAsync(cancellationToken: cancellationToken);
            var publisherAddressDto = _mapper.Map<List<PublisherAddressDto>>(publisherAddres);
            var publisherAddressDtoPaginated = new PaginatedList<PublisherAddressDto>(publisherAddressDto, totalCount,
                listRequest.PagingParams.PageNumber,
                listRequest.PagingParams.PageSize);
            return Result<PaginatedList<PublisherAddressDto>>.Success(publisherAddressDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetPublisherAddressListRequestHandler), e.Message, e);
        }
    }
}