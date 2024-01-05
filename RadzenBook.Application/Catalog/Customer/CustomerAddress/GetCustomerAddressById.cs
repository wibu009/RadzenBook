namespace RadzenBook.Application.Catalog.CustomerAddress;

public class GetCustomerAddressByIdRequest : IRequest<Result<CustomerAddressDto>>
{
    public Guid Id { get; set; }
}

public class GetCustomerAddressByIdRequestHandler : IRequestHandler<GetCustomerAddressByIdRequest, Result<CustomerAddressDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCustomerAddressByIdRequestHandler> _logger;
    private readonly IStringLocalizer _t;

    public GetCustomerAddressByIdRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory logger,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetCustomerAddressByIdRequestHandler>();
        _t = t.Create(typeof(GetCustomerAddressByIdRequestHandler));
    }

    public async Task<Result<CustomerAddressDto>> Handle(GetCustomerAddressByIdRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var CustomerAddress = await _unitOfWork.GetRepository<ICustomerAddressRepository, Domain.Catalog.CustomerAddress, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (CustomerAddress is null)
                return Result<CustomerAddressDto>.Failure("CustomerAddress not found.");
            var CustomerAddressDto = _mapper.Map<CustomerAddressDto>(CustomerAddress);
            return Result<CustomerAddressDto>.Success(CustomerAddressDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetCustomerAddressByIdRequestHandler), e.Message, e);
        }
    }
}