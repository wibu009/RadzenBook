using RadzenBook.Application.Common.Exceptions;

namespace RadzenBook.Application.Catalog.Demo.Query;

public class GetDemoByIdRequest : IRequest<Result<DemoDto>>
{
    public Guid Id { get; set; }
}

public class GetDemoByIdRequestHandler : IRequestHandler<GetDemoByIdRequest, Result<DemoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDemoByIdRequestHandler> _logger;
    private readonly IStringLocalizer _t;
    
    public GetDemoByIdRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, 
        ILoggerFactory logger, 
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetDemoByIdRequestHandler>();
        _t = t.Create(typeof(GetDemoByIdRequestHandler));
    }

    public async Task<Result<DemoDto>> Handle(GetDemoByIdRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var demo = await _unitOfWork.GetRepository<IDemoRepository, Domain.Catalog.Demo, Guid>().GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (demo is null)
            {
                return Result<DemoDto>.Failure("Demo not found.");
            }
            var demoDto = _mapper.Map<DemoDto>(demo);
            return Result<DemoDto>.Success(demoDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(Handle), nameof(GetDemoByIdRequestHandler), e.Message, e);
        }
    }
}