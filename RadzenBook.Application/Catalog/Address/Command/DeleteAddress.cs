namespace RadzenBook.Application.Catalog.Address.Command;

public class DeleteAddressRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}

public class DeleteAddressRequestHandler : IRequestHandler<DeleteAddressRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAddressRequestHandler> _logger;
    private readonly IStringLocalizer _t;

    public DeleteAddressRequestHandler(
        IUnitOfWork unitOfWork,
        ILoggerFactory logger,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _logger = logger.CreateLogger<DeleteAddressRequestHandler>();
        _t = t.Create(typeof(DeleteAddressRequestHandler));
    }

    public async Task<Result<Unit>> Handle(DeleteAddressRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var address = await _unitOfWork.GetRepository<IAddressRepository, Domain.Catalog.Address, Guid>().GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (address is null)
            {
                return Result<Unit>.Failure(_t["Address with id {0} does not exist", request.Id]);
            }
            await _unitOfWork.GetRepository<IAddressRepository, Domain.Catalog.Address, Guid>().DeleteAsync(address, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(DeleteAddressRequestHandler), e.Message, e);
        }
    }
}