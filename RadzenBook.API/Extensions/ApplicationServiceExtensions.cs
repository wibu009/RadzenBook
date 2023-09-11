﻿using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RadzenBook.Common.Settings;
using RadzenBook.Database;
using RadzenBook.Repository.Implements;
using RadzenBook.Repository.Interfaces;
using RadzenBook.Service.Implements.Features;
using RadzenBook.Service.Implements.Infrastructure;
using RadzenBook.Service.Interfaces.Features;
using RadzenBook.Service.Interfaces.Infrastructure;

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
            c.SwaggerDoc( openApiSettings.Info.Version,
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
        services.AddLogging(
            builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
        services.AddSingleton<ILoggerFactory, LoggerFactory>();

        //Add DbContext
        services.AddDbContextPool<RadzenBookDbContext>(options =>
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            var connectionString = env == "Development"
                ? configuration.GetConnectionString("LocalConnection")
                : configuration.GetConnectionString("RemoteConnection");
            
            options.UseSqlServer(connectionString);
        });

        //Add AutoMapper
        services.AddAutoMapper(Assembly.Load("RadzenBook.Contract"));

        //Add FluentValidation
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.Load("RadzenBook.Contract"));

        //Add Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Add Services
        services.AddScoped<IInfrastructureServiceManager, InfrastructureServiceManager>();
        services.AddScoped<IFeaturesServiceManager, FeaturesServiceManager>();

        return services;
    }
}