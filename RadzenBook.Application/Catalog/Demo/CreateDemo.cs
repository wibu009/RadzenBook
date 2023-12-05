using RadzenBook.Application.Common.Security;
using RadzenBook.Domain.Common.Enums;

namespace RadzenBook.Application.Catalog.Demo;

public class CreateDemoRequest : IRequest<Result<Unit>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DemoEnum { get; set; } = string.Empty;
}

public class DemoCreateRequestValidator : CustomValidator<CreateDemoRequest>
{
    public DemoCreateRequestValidator(IStringLocalizer<DemoCreateRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(t["Name is required"])
            .MinimumLength(3).WithMessage(t["Name must be at least {0} characters", 3])
            .MaximumLength(50).WithMessage(t["Name must not exceed {0} characters", 50]);
        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage(t["Description must not exceed {0} characters", 200]);
        RuleFor(x => x.DemoEnum)
            .Must(x => Enum.TryParse<DemoEnum>(x, out _))
            .WithMessage(t["DemoEnum is not valid"]);
    }
}

public class CreateDemoRequestHandler : IRequestHandler<CreateDemoRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<CreateDemoRequestHandler> _logger;

    public CreateDemoRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<CreateDemoRequestHandler>();
        _t = t.Create(typeof(CreateDemoRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(CreateDemoRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var demo = _mapper.Map<Domain.Catalog.Demo>(request);
            demo.CreatedBy = _userAccessor.GetUsername();
            demo.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IDemoRepository, Domain.Catalog.Demo, Guid>()
                .CreateAsync(demo, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Create demo successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(CreateDemoRequestHandler), e.Message, e);
        }
    }
}