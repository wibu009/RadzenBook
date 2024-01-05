namespace RadzenBook.Application.Catalog.Publisher;

public class DeletePublisherRequest : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }
}
public class DeletePublisherRequestValidator : CustomValidator<DeletePublisherRequest>
{
    public DeletePublisherRequestValidator(IStringLocalizer<DeletePublisherRequestValidator> t) : base(t)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(t["Id is required"]);
    }
}
public class DeletePublisherRequestHandler : IRequestHandler<DeletePublisherRequest, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _t;
    private readonly IUserAccessor _userAccessor;
    private readonly ILogger<DeletePublisherRequestHandler> _logger;

    public DeletePublisherRequestHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizerFactory t,
        IInfrastructureServiceManager infrastructureServiceManager,
        ILoggerFactory loggerFactory)
    {
        _unitOfWork = unitOfWork;
        _logger = loggerFactory.CreateLogger<DeletePublisherRequestHandler>();
        _t = t.Create(typeof(DeletePublisherRequestHandler));
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }

    public async Task<Result<Unit>> Handle(DeletePublisherRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var publisher = await _unitOfWork.GetRepository<IPublisherRepository, Domain.Catalog.Publisher, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (publisher == null)
                return Result<Unit>.Failure(_t["Publisher not found"]);
            publisher.ModifiedBy = _userAccessor.GetUsername();
            await _unitOfWork.GetRepository<IPublisherRepository, Domain.Catalog.Publisher, Guid>()
                .SoftDeleteAsync(publisher, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(_t["Publisher deleted successfully"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(DeletePublisherRequestHandler), e.Message, e);
        }
    }
}