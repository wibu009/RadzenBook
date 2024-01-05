
namespace RadzenBook.Application.Catalog.PublisherAddress;

public class GetPublisherAddressByIdRequest : IRequest<Result<PublisherAddressDto>>
{
    public Guid Id { get; set; }
}
public class GetPublisherAddressByIdHandler : IRequestHandler<GetPublisherAddressByIdRequest, Result<PublisherAddressDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPublisherAddressByIdHandler> _logger;
    private readonly IStringLocalizer _t;

    public GetPublisherAddressByIdHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory loggerFactory,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateLogger<GetPublisherAddressByIdHandler>();
        _t = t.Create(typeof(GetPublisherAddressByIdHandler));
    }

    public async Task<Result<PublisherAddressDto>> Handle(GetPublisherAddressByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var genre = await _unitOfWork.GetRepository<IGenreRepository, Domain.Catalog.Genre, Guid>()
                .GetByIdAsync(request.Id, cancellationToken: cancellationToken, includeProperties: "Books");
            if (genre == null)
                return Result<PublisherAddressDto>.Failure(_t["Genre not found"]);
            var genreDto = _mapper.Map<PublisherAddressDto>(genre);
            return Result<PublisherAddressDto>.Success(genreDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetPublisherAddressByIdHandler), e.Message, e);
        }
    }

  
}