namespace RadzenBook.Application.Catalog.CustomerAddress;

public class GetCustomerAddressListRequest : IRequest<Result<PaginatedList<CustomerAddressDto>>>
{
    public PagingParams PagingParams { get; set; } = new();
}

public class GetCustomerAddressListRequestHandler : IRequestHandler<GetCustomerAddressListRequest, Result<PaginatedList<CustomerAddressDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCustomerAddressListRequestHandler> _logger;

    public GetCustomerAddressListRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, ILoggerFactory logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetCustomerAddressListRequestHandler>();
    }

    public async Task<Result<PaginatedList<CustomerAddressDto>>> Handle(GetCustomerAddressListRequest listRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var CustomerAddress = await _unitOfWork.GetRepository<ICustomerAddressRepository, Domain.Catalog.CustomerAddress, Guid>()
                .GetPagedAsync(pageNumber: listRequest.PagingParams.PageNumber,
                    pageSize: listRequest.PagingParams.PageSize,
                    cancellationToken: cancellationToken);
            var totalCount = await _unitOfWork.GetRepository<ICustomerAddressRepository, Domain.Catalog.CustomerAddress, Guid>()
                .CountAsync(cancellationToken: cancellationToken);
            var CustomerAddressDto = _mapper.Map<List<CustomerAddressDto>>(CustomerAddress);
            var CustomerAddressDtoPaginated = new PaginatedList<CustomerAddressDto>(CustomerAddressDto, totalCount,
                listRequest.PagingParams.PageNumber,
                listRequest.PagingParams.PageSize);
            return Result<PaginatedList<CustomerAddressDto>>.Success(CustomerAddressDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetCustomerAddressListRequestHandler), e.Message, e);
        }
    }
}