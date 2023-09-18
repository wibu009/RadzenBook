using AutoMapper;
using Microsoft.Extensions.Logging;
using RadzenBook.Entity;
using RadzenBook.Repository.Interfaces;
using RadzenBook.Service.Interfaces.Features;
using RadzenBook.Service.Interfaces.Infrastructure;
using RadzenBook.Service.Interfaces.Infrastructure.Security;

namespace RadzenBook.Service.Implements.Features;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AddressService> _logger;
    private readonly IUserAccessor _userAccessor;
    
    public AddressService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerFactory logger,
        IInfrastructureServiceManager infrastructureServiceManager)
    {
        _unitOfWork = unitOfWork;
        _addressRepository = _unitOfWork.GetRepository<IAddressRepository, Address, Guid>();
        _mapper = mapper;
        _logger = logger.CreateLogger<AddressService>();
        _userAccessor = infrastructureServiceManager.UserAccessor;
    }
}