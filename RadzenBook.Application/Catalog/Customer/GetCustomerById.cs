namespace RadzenBook.Application.Catalog.Customer;

public class GetCustomerByIdRequest : IRequest<Result<CustomerDto>>
{
    public Guid Id { get; set; }
}

public class GetCustomerByIdRequestHandler : IRequestHandler<GetCustomerByIdRequest, Result<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCustomerByIdRequestHandler> _logger;
    private readonly IStringLocalizer _t;

    public GetCustomerByIdRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory logger,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetCustomerByIdRequestHandler>();
        _t = t.Create(typeof(GetCustomerByIdRequestHandler));
    }

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var Customer = await _unitOfWork.GetRepository<ICustomerRepository, Domain.Catalog.Customer, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (Customer is null)
                return Result<CustomerDto>.Failure("Customer not found.");
            var CustomerDto = _mapper.Map<CustomerDto>(Customer);
            return Result<CustomerDto>.Success(CustomerDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetCustomerByIdRequestHandler), e.Message, e);
        }
    }
}