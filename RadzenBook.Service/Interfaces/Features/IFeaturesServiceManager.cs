namespace RadzenBook.Service.Interfaces.Features;

public interface IFeaturesServiceManager
{
    IDemoService DemoService { get; }
    IAccountService AccountService { get; }
}