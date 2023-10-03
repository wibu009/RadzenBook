using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using OrchardCore.Localization;

namespace RadzenBook.Infrastructure.Localization;

public static class Startup
{
    public static IServiceCollection AddPoLocalization(this IServiceCollection services, IConfiguration configuration)
    {
        var localizationSettings = configuration.GetSection("LocalizationSettings").Get<LocalizationSettings>();
        if (localizationSettings!.EnableLocalization is false ||
            string.IsNullOrEmpty(localizationSettings.ResourcesPath) is true) return services;
        
        services.AddPortableObjectLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            if (localizationSettings.SupportedCultures != null)
            {
                var supportedCultures = localizationSettings.SupportedCultures.Select(x => new CultureInfo(x)).ToList();

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            }

            options.DefaultRequestCulture = new RequestCulture(localizationSettings.DefaultRequestCulture ?? "en-US");
            options.FallBackToParentCultures = localizationSettings.FallbackToParent;
            options.FallBackToParentUICultures = localizationSettings.FallbackToParent;
            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });
        services.AddSingleton<ILocalizationFileLocationProvider, PoFileLocationProvider>();
        
        //set default language in cookie

        return services;
    }
    
    public static IApplicationBuilder UsePoLocalization(this IApplicationBuilder app)
    {
        app.UseRequestLocalization();
        
        return app;
    }
}