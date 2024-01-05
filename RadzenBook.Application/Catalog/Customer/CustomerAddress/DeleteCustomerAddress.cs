namespace RadzenBook.Application.Catalog.CustomerAddress;

public class DeleteCustomerAddressRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteCustomerAddressRequestHandler : IRequestHandler<DeleteCustomerAddressRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<DeleteCustomerAddressRequestHandler> _logger;

    public DeleteCustomerAddressRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<DeleteCustomerAddressRequestHandler>();
        _t = t.Create(typeof(DeleteCustomerAddressRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(DeleteCustomerAddressRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var CustomerAddress = await _unitOfWork.GetRepository<ICustomerAddressRepository, Domain.Catalog.CustomerAddress, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (CustomerAddress is null)
            {
                return Result<Unit>.Failure(_t["CustomerAddress not found."]);
            }

            await _unitOfWork.GetRepository<ICustomerAddressRepository, Domain.Catalog.CustomerAddress, Guid>()
                .DeleteAsync(CustomerAddress, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(DeleteCustomerAddressRequest), nameof(DeleteCustomerAddressRequestHandler), e.Message,
                e);
        }
    }
}