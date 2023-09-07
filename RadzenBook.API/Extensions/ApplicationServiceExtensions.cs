using FirstBlazorProject_BookStore.DataAccess;
using FirstBlazorProject_BookStore.Repository.Implements;
using FirstBlazorProject_BookStore.Repository.Interfaces;
using FirstBlazorProject_BookStore.Service.Implements.Features;
using FirstBlazorProject_BookStore.Service.Interfaces.Features;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FirstBlazorProject_BookStore.API.Extensions;

public static class ApplicationServiceExtensions
{
public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
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

        //Add AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        //Add FluentValidation with assembly
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        //Add DbContext
        services.AddDbContext<RadzenBookDataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ServerConnection"));
        });

        //Add Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Add Services
        services.AddScoped<IDemoService, DemoService>();

        return services;
    }
}