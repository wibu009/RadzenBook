using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RadzenBook.Infrastructure.Identity.Role;
using RadzenBook.Infrastructure.Identity.User;
using RadzenBook.Infrastructure.Persistence;

namespace RadzenBook.Infrastructure.Identity;

public static class Startup
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = false;
                opt.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddRoles<AppRole>()
            .AddUserManager<UserManager<AppUser>>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleValidator<RoleValidator<AppRole>>()
            .AddEntityFrameworkStores<RadzenBookDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}