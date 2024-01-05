using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Customer;

public class UpdateCustomerRequest : IRequest<Result<Unit>>
{
    [JsonIgnore] public Guid Id { get; set; }
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

public class UpdateCustomerRequestValidator : CustomValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator(IStringLocalizer<UpdateCustomerRequestValidator> t) : base(t)
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(50).WithMessage(t["First name must not exceed {0} characters", 50]);
        RuleFor(x => x.LastName)
            .MaximumLength(200).WithMessage(t["Lastname must not exceed {0} characters", 200]);
        RuleFor(x => x.Email)
            .MaximumLength(50).WithMessage(t["Email must not exceed {0} characters", 50]);
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(50).WithMessage(t["Phone must not exceed {0} characters", 50]);
    }
}

public class UpdateCustomerRequestHandler : IRequestHandler<UpdateCustomerRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<UpdateCustomerRequestHandler> _logger;

    public UpdateCustomerRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<UpdateCustomerRequestHandler>();
        _t = t.Create(typeof(UpdateCustomerRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }


    public async Task<Result<Unit>> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var Customer = await _unitOfWork.GetRepository<ICustomerRepository, Domain.Catalog.Customer, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (Customer == null)
            {
                return Result<Unit>.Failure(_t["Customer with id {0} does not exist.", request.Id],
                    (int)HttpStatusCode.NotFound);
            }

            _mapper.Map(request, Customer);
            Customer.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<ICustomerRepository, Domain.Catalog.Customer, Guid>()
                .UpdateAsync(Customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Update Customer successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(UpdateCustomerRequestHandler), e.Message, e);
        }
    }
}