using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.PublisherAddress;

public class UpdatePublisherAddressRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public PublisherAddressType AddressType { get; set; }
    public Guid PublisherId { get; set; }
}
public class UpdatePublisherAddressRequestValidator : CustomValidator<UpdatePublisherAddressRequest>
{
    public UpdatePublisherAddressRequestValidator(IStringLocalizer<UpdatePublisherAddressRequestValidator> t) : base(t)
    {
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
        RuleFor(x => x.AddressType)
         .NotEmpty().WithMessage(t["AddressType is required"]);
    }
}
public class UpdatePublisherAddressRequestHandler : IRequestHandler<UpdatePublisherAddressRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<UpdatePublisherAddressRequestHandler> _logger;

    public UpdatePublisherAddressRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<UpdatePublisherAddressRequestHandler>();
        _t = t.Create(typeof(UpdatePublisherAddressRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(UpdatePublisherAddressRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var publisherAddress = _mapper.Map<Domain.Catalog.PublisherAddress>(request);
            publisherAddress.CreatedBy = _userAccessor.GetUsername();
            publisherAddress.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IPublisherAddressRepository, Domain.Catalog.PublisherAddress, Guid>()
                .CreateAsync(publisherAddress, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["PublisherAddress updated successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(UpdatePublisherAddressRequestHandler), e.Message, e);
        }
    }
}
