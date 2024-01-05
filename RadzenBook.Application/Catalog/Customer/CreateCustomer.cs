using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Customer;

public class CreateCustomerRequest : IRequest<Result<Unit>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender Gender { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool IsGuest { get; set; }
    public Guid? UserId { get; set; }
    public Guid? CartId { get; set; }

}

public class CustomerCreateRequestValidator : CustomValidator<CreateCustomerRequest>
{
    public CustomerCreateRequestValidator(IStringLocalizer<CustomerCreateRequestValidator> t) : base(t)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(t["First name is required"])
            .MaximumLength(50).WithMessage(t["Name must not exceed {0} characters", 50]);
        RuleFor(x => x.LastName)
            .MaximumLength(2500).WithMessage(t["Last name must not exceed {0} characters", 2500]);
        RuleFor(x => x.Email)
            .MaximumLength(2500).WithMessage(t["Email must not exceed {0} characters", 2500]);
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(2500).WithMessage(t["Phone number must not exceed {0} characters", 2500]);
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).When(x => x.DateOfBirth.HasValue)
            .WithMessage(t["DateOfBirth must be less than today"]);
    }
}

public class CreateCustomerRequestHandler : IRequestHandler<CreateCustomerRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<CreateCustomerRequestHandler> _logger;

    public CreateCustomerRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<CreateCustomerRequestHandler>();
        _t = t.Create(typeof(CreateCustomerRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var Customer = _mapper.Map<Domain.Catalog.Customer>(request);
            Customer.CreatedBy = _userAccessor.GetUsername();
            Customer.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<ICustomerRepository, Domain.Catalog.Customer, Guid>()
                .CreateAsync(Customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Create Customer successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(CreateCustomerRequestHandler), e.Message, e);
        }
    }
}