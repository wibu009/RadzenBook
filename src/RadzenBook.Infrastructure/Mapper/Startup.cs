namespace RadzenBook.Infrastructure.Mapper;

public static class Startup
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic));
        
        return services;
    }
}