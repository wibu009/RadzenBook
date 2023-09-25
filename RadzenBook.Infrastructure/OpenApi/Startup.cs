using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace RadzenBook.Infrastructure.OpenApi;

public static class Startup
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            var openApiSettings = configuration.GetSection("OpenApiSettings").Get<OpenApiSettings>();
            options.SwaggerDoc( openApiSettings!.Info.Version,
                new OpenApiInfo
                {
                    Title = openApiSettings.Info.Title,
                    Version = openApiSettings.Info.Version,
                    Description = openApiSettings.Info.Description,
                    Contact = new OpenApiContact
                    {
                        Name = openApiSettings.Info.Contact.Name,
                        Email = openApiSettings.Info.Contact.Email,
                    },
                    License = new OpenApiLicense()
                    {
                        Name = openApiSettings.Info.License.Name,
                        Url = new Uri(openApiSettings.Info.License.Url),
                    },
                    TermsOfService = new Uri(openApiSettings.Info.TermsOfService),
                });
            options.EnableAnnotations();
            options.OperationFilter<AcceptLanguageHeaderFilter>();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = openApiSettings.Security.Description,
                Name = openApiSettings.Security.Name,
                Type = SecuritySchemeType.Http,
                BearerFormat = openApiSettings.Security.BearerFormat,
                Scheme = openApiSettings.Security.Scheme,
            });
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                                {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        new string[] { }
                    }
                });
        });
        
        return services;
    }
    
    public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
        return app;
    }
}