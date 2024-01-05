namespace RadzenBook.Application.Catalog.Publisher;

public class UpdatePublisherRequest : IRequest<Result<Unit>>
{
    [JsonIgnore] public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}
public class UpdatePublisherRequestValidator : CustomValidator<UpdatePublisherRequest>
{
    public UpdatePublisherRequestValidator(IStringLocalizer<UpdatePublisherRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(t["Name is required"])
            .MaximumLength(50).WithMessage(t["Name must not exceed {0} characters", 50]);
        RuleFor(x => x.Description)
            .MaximumLength(2500).WithMessage(t["Description must not exceed {0} characters", 2500]);
        RuleFor(x => x.PhoneNumber)
          .NotEmpty().WithMessage(t["PhoneNumber is required"])
          .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
        RuleFor(x => x.Email)
           .NotEmpty().WithMessage(t["Email is required"])
           .EmailAddress().WithMessage(t["Invalid email format"]);
    }
}
public class UpdatePublisherRequestHandler : IRequestHandler<UpdatePublisherRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<UpdatePublisherRequestHandler> _logger;

    public UpdatePublisherRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<UpdatePublisherRequestHandler>();
        _t = t.Create(typeof(UpdatePublisherRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(UpdatePublisherRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var publisher = await _unitOfWork.GetRepository<IPublisherRepository, Domain.Catalog.Publisher, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (publisher == null)
                return Result<Unit>.Failure(_t["Publisher not found"]);
            publisher.ModifiedBy = _userAccessor.GetUsername();
            _mapper.Map(request, publisher);
            publisher.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Publisher updated successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(UpdatePublisherRequestHandler), e.Message, e);
        }
    }
}