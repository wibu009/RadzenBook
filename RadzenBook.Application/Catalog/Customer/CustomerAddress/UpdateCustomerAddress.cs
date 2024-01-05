using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.CustomerAddress;

public class UpdateCustomerAddressRequest : IRequest<Result<Unit>>
{
    [JsonIgnore] public Guid Id { get; set; }
    public string? ConsigneeName { get; set; }
    public string? ConsigneePhoneNumber { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public CustomerAddressType AddressType { get; set; }
    public bool IsDefault { get; set; }
    public Guid? CustomerId { get; set; }

}

public class UpdateCustomerAddressRequestValidator : CustomValidator<UpdateCustomerAddressRequest>
{
    public UpdateCustomerAddressRequestValidator(IStringLocalizer<UpdateCustomerAddressRequestValidator> t) : base(t)
    {
        RuleFor(x => x.ConsigneeName)
            .MaximumLength(50).WithMessage(t["ConsigneeName must not exceed {0} characters", 50]);
        RuleFor(x => x.ConsigneePhoneNumber)
            .MaximumLength(50).WithMessage(t["ConsigneePhoneNumber must not exceed {0} characters", 50]);
        RuleFor(x => x.AddressLine1)
            .MaximumLength(50).WithMessage(t["AddressLine1 must not exceed {0} characters", 50]);
        RuleFor(x => x.AddressLine2)
            .MaximumLength(50).WithMessage(t["AddressLine2 must not exceed {0} characters", 50]);
        RuleFor(x => x.City)
            .MaximumLength(200).WithMessage(t["City must not exceed {0} characters", 200]);  
        RuleFor(x => x.State)
            .MaximumLength(200).WithMessage(t["State must not exceed {0} characters", 200]);  
        RuleFor(x => x.Country)
            .MaximumLength(200).WithMessage(t["Country must not exceed {0} characters", 200]);
        RuleFor(x => x.ZipCode)
            .MaximumLength(200).WithMessage(t["ZipCode must not exceed {0} characters", 200]);       
    }
}

public class UpdateCustomerAddressRequestHandler : IRequestHandler<UpdateCustomerAddressRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<UpdateCustomerAddressRequestHandler> _logger;

    public UpdateCustomerAddressRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<UpdateCustomerAddressRequestHandler>();
        _t = t.Create(typeof(UpdateCustomerAddressRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }


    public async Task<Result<Unit>> Handle(UpdateCustomerAddressRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var CustomerAddress = await _unitOfWork.GetRepository<ICustomerAddressRepository, Domain.Catalog.CustomerAddress, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (CustomerAddress == null)
            {
                return Result<Unit>.Failure(_t["CustomerAddress with id {0} does not exist.", request.Id],
                    (int)HttpStatusCode.NotFound);
            }

            _mapper.Map(request, CustomerAddress);
            CustomerAddress.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<ICustomerAddressRepository, Domain.Catalog.CustomerAddress, Guid>()
                .UpdateAsync(CustomerAddress, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Update CustomerAddress successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(UpdateCustomerAddressRequestHandler), e.Message, e);
        }
    }
}