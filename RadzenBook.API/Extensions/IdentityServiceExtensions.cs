using Microsoft.AspNetCore.Identity;
using RadzenBook.Database;
using RadzenBook.Entity;

namespace RadzenBook.Api.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<AppRole>()
            .AddUserManager<UserManager<AppUser>>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleValidator<RoleValidator<AppRole>>()
            .AddEntityFrameworkStores<RadzenBookDataContext>();

        return services;
    }
}