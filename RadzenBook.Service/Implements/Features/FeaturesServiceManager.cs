using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RadzenBook.Entity;
using RadzenBook.Repository.Interfaces;
using RadzenBook.Service.Interfaces.Features;
using RadzenBook.Service.Interfaces.Infrastructure;

namespace RadzenBook.Service.Implements.Features;

public class FeaturesServiceManager : IFeaturesServiceManager
{
    private readonly Lazy<IAccountService> _accountService;
    private readonly Lazy<IDemoService> _demoService;
    private readonly Lazy<IAddressService> _addressService;
    private readonly Lazy<IPhotoService> _photoService;

    public FeaturesServiceManager(
        ILoggerFactory loggerFactory, 
        IMapper mapper, 
        IUnitOfWork unitOfWork,
        IInfrastructureServiceManager infrastructureServiceManager,
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager, 
        RoleManager<AppRole> roleManager)
    {
        _accountService = new Lazy<IAccountService>(() =>
            new AccountService(userManager, signInManager, infrastructureServiceManager, loggerFactory.CreateLogger<AccountService>()));
        _demoService = new Lazy<IDemoService>(() =>
            new DemoService(unitOfWork, mapper, loggerFactory.CreateLogger<DemoService>(), infrastructureServiceManager));
        _addressService = new Lazy<IAddressService>(() =>
            new AddressService(unitOfWork, mapper, loggerFactory.CreateLogger<AddressService>(), infrastructureServiceManager));
        _photoService = new Lazy<IPhotoService>(() =>
            new PhotoService(unitOfWork, mapper, loggerFactory.CreateLogger<PhotoService>(), infrastructureServiceManager));
    }

    public IAccountService AccountService => _accountService.Value;
    public IDemoService DemoService => _demoService.Value;
    public IAddressService AddressService => _addressService.Value;
    public IPhotoService PhotoService => _photoService.Value;
}