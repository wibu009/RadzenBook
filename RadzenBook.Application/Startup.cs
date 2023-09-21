using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace RadzenBook.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}