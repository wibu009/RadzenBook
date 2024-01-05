namespace RadzenBook.Application.Catalog.Customer;

public class DeleteCustomerRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteCustomerRequestHandler : IRequestHandler<DeleteCustomerRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<DeleteCustomerRequestHandler> _logger;

    public DeleteCustomerRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<DeleteCustomerRequestHandler>();
        _t = t.Create(typeof(DeleteCustomerRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var Customer = await _unitOfWork.GetRepository<ICustomerRepository, Domain.Catalog.Customer, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (Customer is null)
            {
                return Result<Unit>.Failure(_t["Customer not found."]);
            }

            await _unitOfWork.GetRepository<ICustomerRepository, Domain.Catalog.Customer, Guid>()
                .DeleteAsync(Customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(DeleteCustomerRequest), nameof(DeleteCustomerRequestHandler), e.Message,
                e);
        }
    }
}