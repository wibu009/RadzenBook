﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RadzenBook.Database;
using RadzenBook.Repository.Implements;
using RadzenBook.Repository.Interfaces;
using RadzenBook.Service.Implements.Features;
using RadzenBook.Service.Interfaces.Features;

namespace RadzenBook.Api.Extensions;

public static class ApplicationServiceExtensions
{
public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Add basic services
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        
        //Add HTTP services
        services.AddHttpContextAccessor();
        
        //Add Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "RadzenBook.API",
                    Version = "v1",
                    Description = "RadzenBook.API Swagger Surface",
                    Contact = new OpenApiContact
                    {
                        Name = "AveTeam",
                        Email = "kienct.work@gmail.com",
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = " Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
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

        //Add Cors
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders("WWW-Authenticate", "Pagination")
                    .WithOrigins("http://localhost:5000", "https://localhost:5001");
            });
        });

        //Add DbContext
        services.AddDbContextPool<RadzenBookDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ServerConnection"));
        });

        //Add Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Add Services
        services.AddScoped<IDemoService, DemoService>();
        
        //Add AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}