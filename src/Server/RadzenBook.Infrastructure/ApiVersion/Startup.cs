using Microsoft.AspNetCore.Mvc.Versioning;

namespace RadzenBook.Infrastructure.ApiVersion;

public static class Startup
{
    public static IServiceCollection AddVersion(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            opt.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("X-Version"),
                new QueryStringApiVersionReader("api-version"),
                new UrlSegmentApiVersionReader());
        });
        
        services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });
        
        return services;
    }
}