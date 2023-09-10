namespace RadzenBook.Service.Interfaces.Infrastructure.Security;

public interface IUserAccessor
{
    string GetUsername();
    string GetUserEmail();
    string GetUserId();
}