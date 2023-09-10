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

    public FeaturesServiceManager(
        ILoggerFactory loggerFactory,
        IMapper mapper, IUnitOfWork unitOfWork,
        IInfrastructureServiceManager infrastructureServiceManager,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
    {
        _accountService = new Lazy<IAccountService>(() =>
            new AccountService(userManager, signInManager, infrastructureServiceManager, loggerFactory.CreateLogger<AccountService>()));
        _demoService = new Lazy<IDemoService>(() =>
            new DemoService(unitOfWork, mapper, loggerFactory.CreateLogger<DemoService>()));
    }

    public IAccountService AccountService => _accountService.Value;
    public IDemoService DemoService => _demoService.Value;
}