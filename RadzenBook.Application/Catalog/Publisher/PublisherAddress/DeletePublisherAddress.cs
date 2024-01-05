namespace RadzenBook.Application.Catalog.PublisherAddress;

public class DeletePublisherAddressRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}
public class DeletePublisherAddressRequestValidator : CustomValidator<DeletePublisherAddressRequest>
{
    public DeletePublisherAddressRequestValidator(IStringLocalizer<DeletePublisherAddressRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(t["Id is required"]);
    }
}

public class DeletePublisherAddressRequestHandler : IRequestHandler<DeletePublisherAddressRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<DeletePublisherAddressRequestHandler> _logger;

    public DeletePublisherAddressRequestHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _logger = loggerFactory.CreateLogger<DeletePublisherAddressRequestHandler>();
        _t = t.Create(typeof(DeletePublisherAddressRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(DeletePublisherAddressRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var publisherAddress = await _unitOfWork.GetRepository<IPublisherAddressRepository, Domain.Catalog.PublisherAddress, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (publisherAddress == null)
                return Result<Unit>.Failure(_t["PublisherAddress not found"]);
            publisherAddress.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IPublisherAddressRepository, Domain.Catalog.PublisherAddress, Guid>()
                .SoftDeleteAsync(publisherAddress, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["PublisherAddress deleted successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(DeletePublisherAddressRequestHandler), e.Message, e);
        }
    }
}