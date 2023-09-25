namespace RadzenBook.Application.Common.Security;

public interface IUserAccessor
{
    string GetUsername();
    string GetUserId();
    string GetUserRole();
}