namespace RadzenBook.Application.Common.Auth;

public interface IUserAccessor
{
    string GetUsername();
    string GetUserEmail();
    string GetUserId();
    string GetUserRole();
}