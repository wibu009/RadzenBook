using System.Globalization;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrchardCore.Localization;
using RadzenBook.Common.Settings;
using RadzenBook.Database;
using RadzenBook.Persistence;
using RadzenBook.Repository.Implements;
using RadzenBook.Repository.Interfaces;
using RadzenBook.Service.Implements.Features;
using RadzenBook.Service.Implements.Infrastructure;
using RadzenBook.Service.Implements.Infrastructure.Localization;
using RadzenBook.Service.Interfaces.Features;
using RadzenBook.Service.Interfaces.Infrastructure;
using Serilog;

namespace RadzenBook.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Add basic services
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        
        //Add HTTP services
        services.AddHttpContextAccessor();
        
        //Add Swagger
        services.AddSwaggerGen(c =>
        {
            var openApiSettings = configuration.GetSection("OpenApiSettings").Get<OpenApiSettings>();
            c.SwaggerDoc( openApiSettings!.Info.Version,
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
            c.EnableAnnotations();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = openApiSettings.Security.Description,
                Name = openApiSettings.Security.Name,
                Type = SecuritySchemeType.Http,
                BearerFormat = openApiSettings.Security.BearerFormat,
                Scheme = openApiSettings.Security.Scheme,
            });
            c.AddSecurityRequirement(
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

        //Add Logging
        services.AddLogging();
        services.AddSingleton<ILoggerFactory, LoggerFactory>();

        //Add DbContext
        services.AddDbContextPool<RadzenBookDbContext>(options =>
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            var connectionString = env == "Development"
                ? configuration.GetConnectionString("LocalConnection")
                : configuration.GetConnectionString("RemoteConnection");

            if (string.IsNullOrEmpty(connectionString))
                throw new NullReferenceException("Connection string is missing");

            options.UseSqlServer(connectionString);
        });

        //Add AutoMapper
        services.AddAutoMapper(Assembly.Load("RadzenBook.Contract"));

        //Add FluentValidation
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.Load("RadzenBook.Contract"));
        
        //Add Cache
        services.AddMemoryCache();
        
        //Add Localization
        services.AddPortableObjectLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("vi-VN"),
            };
            
            options.SetDefaultCulture("vi");
            options.DefaultRequestCulture = new RequestCulture("vi-VN");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.FallBackToParentCultures = true;
            options.FallBackToParentUICultures = true;
            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });
        services.AddSingleton<ILocalizationFileLocationProvider, PoFileLocationProvider>();

        //Add Repositories
        services.AddScoped<IUnitOfWork>(provider =>
            new UnitOfWork(provider.GetRequiredService<RadzenBookDbContext>()));

        //Add Services
        services.AddScoped<IInfrastructureServiceManager, InfrastructureServiceManager>();
        services.AddScoped<IFeaturesServiceManager, FeaturesServiceManager>();

        return services;
    }
}