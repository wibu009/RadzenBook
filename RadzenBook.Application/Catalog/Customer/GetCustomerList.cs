namespace RadzenBook.Application.Catalog.Customer;

public class GetCustomerListRequest : IRequest<Result<PaginatedList<CustomerDto>>>
{
    public PagingParams PagingParams { get; set; } = new();
}

public class GetCustomerListRequestHandler : IRequestHandler<GetCustomerListRequest, Result<PaginatedList<CustomerDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCustomerListRequestHandler> _logger;

    public GetCustomerListRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, ILoggerFactory logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetCustomerListRequestHandler>();
    }

    public async Task<Result<PaginatedList<CustomerDto>>> Handle(GetCustomerListRequest listRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var Customers = await _unitOfWork.GetRepository<ICustomerRepository, Domain.Catalog.Customer, Guid>()
                .GetPagedAsync(pageNumber: listRequest.PagingParams.PageNumber,
                    pageSize: listRequest.PagingParams.PageSize,
                    cancellationToken: cancellationToken);
            var totalCount = await _unitOfWork.GetRepository<ICustomerRepository, Domain.Catalog.Customer, Guid>()
                .CountAsync(cancellationToken: cancellationToken);
            var CustomersDto = _mapper.Map<List<CustomerDto>>(Customers);
            var CustomersDtoPaginated = new PaginatedList<CustomerDto>(CustomersDto, totalCount,
                listRequest.PagingParams.PageNumber,
                listRequest.PagingParams.PageSize);
            return Result<PaginatedList<CustomerDto>>.Success(CustomersDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetCustomerListRequestHandler), e.Message, e);
        }
    }
}