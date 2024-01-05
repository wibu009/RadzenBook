namespace RadzenBook.Application.Catalog.Publisher;
public class GetPublisherByIdRequest : IRequest<Result<PublisherDto>>
{
    public Guid Id { get; set; }
}

public class GetPublisherByIdHandler : IRequestHandler<GetPublisherByIdRequest, Result<PublisherDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPublisherByIdHandler> _logger;
    private readonly IStringLocalizer _t;

    public GetPublisherByIdHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<GetPublisherByIdHandler>();
        _t = t.Create(typeof(GetPublisherByIdHandler));
    }

    public async Task<Result<PublisherDto>> Handle(GetPublisherByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var publisher = await _unitOfWork.GetRepository<IPublisherRepository, Domain.Catalog.Publisher, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken, includeProperties: "Books");
            if (publisher == null)
                return Result<PublisherDto>.Failure(_t["Publisher not found"]);
            var publisherDto = _mapper.Map<PublisherDto>(publisher);
            return Result<PublisherDto>.Success(publisherDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetPublisherByIdHandler), e.Message, e);
        }
    }
}