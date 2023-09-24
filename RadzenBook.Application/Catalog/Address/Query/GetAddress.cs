using RadzenBook.Application.Common.Exceptions;

namespace RadzenBook.Application.Catalog.Address.Query;

public class GetAddressRequest : IRequest<Result<PaginatedList<AddressDto>>>
{
    public PagingParams PagingParams { get; set; } = new();
}

public class GetAddressRequestHandler : IRequestHandler<GetAddressRequest, Result<PaginatedList<AddressDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAddressRequestHandler> _logger;
    private readonly IStringLocalizer _t;

    public GetAddressRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory logger,
        IStringLocalizerFactory t)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger.CreateLogger<GetAddressRequestHandler>();
        _t = t.Create(typeof(GetAddressRequestHandler));
    }

    public async Task<Result<PaginatedList<AddressDto>>> Handle(GetAddressRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var address = await _unitOfWork.GetRepository<IAddressRepository, Domain.Catalog.Address, Guid>().GetPagedAsync(pageNumber: request.PagingParams.PageNumber, pageSize: request.PagingParams.PageSize, cancellationToken: cancellationToken);
            var totalCount = await _unitOfWork.GetRepository<IAddressRepository, Domain.Catalog.Address, Guid>().CountAsync(cancellationToken: cancellationToken);
            var addressDto = _mapper.Map<List<AddressDto>>(address);
            var addressDtoPaginated = new PaginatedList<AddressDto>(addressDto, totalCount, request.PagingParams.PageNumber, request.PagingParams.PageSize);
            return Result<PaginatedList<AddressDto>>.Success(addressDtoPaginated);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw ServiceException.Create(nameof(Handle), nameof(GetAddressRequestHandler), e.Message, e);
        }
    }
}