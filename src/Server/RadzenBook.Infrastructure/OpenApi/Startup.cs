using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace RadzenBook.Infrastructure.OpenApi;

public static class Startup
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            var openApiSettings = configuration.GetSection("OpenApiSettings").Get<OpenApiSettings>()!;
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = openApiSettings.Info.Title,
                    Version = description.GroupName,
                    Description = openApiSettings.Info.Description,
                    Contact = new OpenApiContact
                    {
                        Name = openApiSettings.Info.Contact.Name,
                        Email = openApiSettings.Info.Contact.Email,
                    },
                    License = new OpenApiLicense
                    {
                        Name = openApiSettings.Info.License.Name,
                        Url = new Uri(openApiSettings.Info.License.Url),
                    },
                });
            }
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
        var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
            {
                foreach (var description in provider!.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });
        
        return app;
    }
}