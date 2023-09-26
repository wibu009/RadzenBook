namespace RadzenBook.Application.Catalog.Address.Query;

public class GetAddressByIdRequest : IRequest<Result<AddressDto>>
{
    public Guid Id { get; set; }
}

public class GetAddressByIdRequestHandler : IRequestHandler<GetAddressByIdRequest, Result<AddressDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAddressByIdRequestHandler> _logger;
    private readonly IStringLocalizer _t;

    public GetAddressByIdRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory logger,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetAddressByIdRequestHandler>();
        _t = t.Create(typeof(GetAddressByIdRequestHandler));
    }

    public async Task<Result<AddressDto>> Handle(GetAddressByIdRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var address = await _unitOfWork.GetRepository<IAddressRepository, Domain.Catalog.Address, Guid>().GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (address is null)
            {
                return Result<AddressDto>.Failure(_t["Address with id {0} does not exist", request.Id]);
            }
            var addressDto = _mapper.Map<AddressDto>(address);
            return Result<AddressDto>.Success(addressDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw HandleRequestException.Create(nameof(Handle), nameof(GetAddressByIdRequestHandler), e.Message, e);
        }
    }
}