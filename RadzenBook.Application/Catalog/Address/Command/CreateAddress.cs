using RadzenBook.Application.Common.Security;

namespace RadzenBook.Application.Catalog.Address.Command;

public class CreateAddressRequest : IRequest<Result<Unit>>
{
    public string FullName { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string? Email { get; set; } = default!;
    public Guid? AppUserId { get; set; } = default!;
}

public class CreateAddressRequestValidator : CustomValidator<CreateAddressRequest>
{
    public CreateAddressRequestValidator(IStringLocalizer<CreateAddressRequestValidator> t) : base(t)
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(t["FullName is required"])
            .MinimumLength(3).WithMessage(t["FullName must be at least {0} characters", 3])
            .MaximumLength(50).WithMessage(t["FullName must not exceed {0} characters", 100]);
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage(t["Street is required"])
            .MinimumLength(3).WithMessage(t["Street must be at least {0} characters", 3])
            .MaximumLength(200).WithMessage(t["Street must not exceed {0} characters", 200]);
        RuleFor(x => x.City)
            .NotEmpty().WithMessage(t["City is required"])
            .MinimumLength(3).WithMessage(t["City must be at least {0} characters", 3])
            .MaximumLength(100).WithMessage(t["City must not exceed {0} characters", 100]);
        RuleFor(x => x.State)
            .NotEmpty().WithMessage(t["State is required"])
            .MinimumLength(3).WithMessage(t["State must be at least {0} characters", 3])
            .MaximumLength(100).WithMessage(t["State must not exceed {0} characters", 100]);
        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage(t["ZipCode is required"])
            .Matches(@"^[0-9]+$").WithMessage(t["ZipCode is not valid"])
            .MinimumLength(3).WithMessage(t["ZipCode must be at least {0} characters", 3])
            .MaximumLength(100).WithMessage(t["ZipCode must not exceed {0} characters", 100]);
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage(t["Country is required"])
            .MinimumLength(3).WithMessage(t["Country must be at least {0} characters", 3])
            .MaximumLength(100).WithMessage(t["Country must not exceed {0} characters", 100]);
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(t["PhoneNumber is required"])
            .Matches(@"^(0[1-9]{1}[0-9]{8}|0[1-9]{1}[0-9]{9})$").WithMessage(t["Phone Number is not valid"]);
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage(t["Email is not valid"]);
    }
}

public class CreateAddressRequestHandler : IRequestHandler<CreateAddressRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<CreateAddressRequestHandler> _logger;

    public CreateAddressRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<CreateAddressRequestHandler>();
        _t = t.Create(typeof(CreateAddressRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(CreateAddressRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var address = _mapper.Map<Domain.Catalog.Address>(request);
            address.CreatedBy = _userAccessor.GetUsername();
            address.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IAddressRepository, Domain.Catalog.Address, Guid>().CreateAsync(address, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Create address successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Create address error");
            throw HandleRequestException.Create(nameof(Handle), nameof(CreateAddressRequestHandler), e.Message, e);
        }
    }
}