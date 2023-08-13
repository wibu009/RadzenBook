using FirstBlazorProject_BookStore.DataAccess.Context;
using FirstBlazorProject_BookStore.Repository.Unit;
using FirstBlazorProject_BookStore.Service.Demo;
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
        });

        //Context
        services.AddDbContext<DataContext>(options =>
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