namespace RadzenBook.Infrastructure.Security.Header;

public static class SecurityHeadersRegister
{
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