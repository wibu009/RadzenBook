using RadzenBook.Application.Common.Exceptions;

namespace RadzenBook.Application.Catalog.Demo.Query;

public class GetAllDemoRequest : IRequest<Result<PaginatedList<DemoDto>>>
{
    
}

public class GetAllDemoRequestHandler : IRequestHandler<GetAllDemoRequest, Result<PaginatedList<DemoDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllDemoRequestHandler> _logger;
    
    public GetAllDemoRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, ILoggerFactory logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetAllDemoRequestHandler>();
    }

    public async Task<Result<PaginatedList<DemoDto>>> Handle(GetAllDemoRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var demos = await _unitOfWork.GetRepository<IDemoRepository, Domain.Catalog.Demo, Guid>().GetAsync(cancellationToken: cancellationToken);
            var size = demos.Count;
            var demosDto = _mapper.Map<List<DemoDto>>(demos);
            var demosDtoPaginated = await PaginatedList<DemoDto>.CreateAsync(demosDto, 1, size);
            return Result<PaginatedList<DemoDto>>.Success(demosDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(Handle), nameof(GetAllDemoRequestHandler), e.Message, e);
        }
    }
}