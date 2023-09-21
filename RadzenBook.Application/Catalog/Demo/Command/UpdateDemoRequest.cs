using System.Net;
using System.Text.Json.Serialization;

namespace RadzenBook.Application.Catalog.Demo.Command;

public class UpdateDemoRequest : IRequest<Result<Unit>>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DemoEnum { get; set; } = string.Empty;
    
    public static UpdateDemoRequest SetId(Guid id, UpdateDemoRequest request)
    {
        request.Id = id;
        return request;
    }
}

public class UpdateDemoRequestValidator : CustomValidator<UpdateDemoRequest>
{
    public UpdateDemoRequestValidator(IStringLocalizer<UpdateDemoRequestValidator> t)
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage(t["Name must not exceed {0} characters", 50]);
        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage(t["Description must not exceed {0} characters", 200]);
        RuleFor(x => x.DemoEnum)
            .Must(x => Enum.TryParse<DemoEnum>(x, out _)).WithMessage(t["DemoEnum is not valid"]);
    }
}

public class UpdateDemoRequestHandler : IRequestHandler<UpdateDemoRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<UpdateDemoRequestHandler> _logger;

    public UpdateDemoRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<UpdateDemoRequestHandler>();
        _t = t.Create(typeof(UpdateDemoRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }
    

    public async Task<Result<Unit>> Handle(UpdateDemoRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var demo = await _unitOfWork.GetRepository<IDemoRepository, Domain.Entities.Demo, Guid>().GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (demo == null)
            {
                return Result<Unit>.Failure(_t["Demo with id {0} does not exist.", request.Id], (int)HttpStatusCode.NotFound);
            }
            _mapper.Map(request, demo);
            demo.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IDemoRepository, Domain.Entities.Demo, Guid>().UpdateAsync(demo, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Update demo successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(Handle), nameof(UpdateDemoRequestHandler), e.Message, e);
        }
    }
}