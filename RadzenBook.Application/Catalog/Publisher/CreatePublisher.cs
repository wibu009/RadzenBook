

namespace RadzenBook.Application.Catalog.Publisher;

public class CreatePublisherRequest : IRequest<Result<Unit>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

}


public class CreatePublisherRequestValidator : CustomValidator<CreatePublisherRequest>
{
    public CreatePublisherRequestValidator(IStringLocalizer<CreatePublisherRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(t["Name is required"])
            .MaximumLength(50).WithMessage(t["Name must not exceed {0} characters", 50]);
        RuleFor(x => x.Description)
            .MaximumLength(2500).WithMessage(t["Description must not exceed {0} characters", 2500]);
        RuleFor(x => x.PhoneNumber)
           .NotEmpty().WithMessage(t["PhoneNumber is required"]);

        RuleFor(x => x.Email)
           .NotEmpty().WithMessage(t["Email is required"])
           .EmailAddress().WithMessage(t["Invalid email format"]);
    }
}



public class CreatePublisherRequestHandler : IRequestHandler<CreatePublisherRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<CreatePublisherRequestHandler> _logger;

    public CreatePublisherRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<CreatePublisherRequestHandler>();
        _t = t.Create(typeof(CreatePublisherRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(CreatePublisherRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var publisher = _mapper.Map<Domain.Catalog.Publisher>(request);
            publisher.CreatedBy = _userAccessor.GetUsername();
            publisher.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IPublisherRepository, Domain.Catalog.Publisher, Guid>()
                .CreateAsync(publisher, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(_t["Publisher created successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(CreatePublisherRequestHandler), e.Message, e);
        }
    }
}
