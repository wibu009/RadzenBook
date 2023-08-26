using FirstBlazorProject_BookStore.DataAccess.Context;
using FirstBlazorProject_BookStore.Repository.Unit;
using FirstBlazorProject_BookStore.Service.Demo;
using FirstBlazorProject_BookStore.Service.Mapper;
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
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "FirstBlazorProject_BookStore.API", Version = "v1"});
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

        services.AddAutoMapper(typeof(MappingProfiles).Assembly);

        //Context
        services.AddDbContext<BookStoreDataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        //Add Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Add Services
        services.AddScoped<IDemoService, DemoService>();

        return services;
    }
}