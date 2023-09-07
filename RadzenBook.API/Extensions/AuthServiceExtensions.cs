using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace RadzenBook.Api.Extensions;

public static class AuthServiceExtensions
{
    public static IServiceCollection AddAuthServices (this IServiceCollection services, IConfiguration config)
    {
        var jwtSettings = config.GetSection("JwtSettings");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
        });

        return services;
    }
}