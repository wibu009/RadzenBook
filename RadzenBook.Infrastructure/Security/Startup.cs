using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using RadzenBook.Application.Common.Security;
using RadzenBook.Application.Identity.Token;
using RadzenBook.Infrastructure.Identity.Token;

namespace RadzenBook.Infrastructure.Security;

public static class Startup
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("TokenSettings:Key").Value!)),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services.AddAuthorization();

        services.AddTransient<ITokenService, TokenService>();

        return services;
    }
    
    public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        
        return app;
    }
    
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        app.UseXContentTypeOptions();
        app.UseReferrerPolicy(opt => opt.NoReferrer());
        app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
        app.UseXfo(opt => opt.Deny());
        app.UseCsp(opt => opt
            .BlockAllMixedContent()
            .StyleSources(s => s.Self())
            .FontSources(s => s.Self())
            .FormActions(s => s.Self())
            .FrameAncestors(s => s.Self())
            .ImageSources(s => s.Self())
            .ScriptSources(s => s.Self())
        );

        app.UseHttpsRedirection();
        app.UseHsts();
        
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
            await next();
        });
        
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
            await next.Invoke();
        });

        return app;
    }
}