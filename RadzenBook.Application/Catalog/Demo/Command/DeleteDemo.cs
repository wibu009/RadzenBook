using RadzenBook.Application.Common.Exceptions;

namespace RadzenBook.Application.Catalog.Demo.Command;

public class DeleteDemoRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteDemoRequestHandler : IRequestHandler<DeleteDemoRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<DeleteDemoRequestHandler> _logger;

    public DeleteDemoRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<DeleteDemoRequestHandler>();
        _t = t.Create(typeof(DeleteDemoRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(DeleteDemoRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var demo = await _unitOfWork.GetRepository<IDemoRepository, Domain.Catalog.Demo, Guid>().GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (demo is null)
            {
                return Result<Unit>.Failure(_t["Demo not found."]);
            }

            await _unitOfWork.GetRepository<IDemoRepository, Domain.Catalog.Demo, Guid>().DeleteAsync(demo, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(DeleteDemoRequest), nameof(DeleteDemoRequestHandler), e.Message, e);
        }
    }
}