using FluentValidation;
using FluentValidation.AspNetCore;

namespace RadzenBook.Infrastructure.Validations;

public static class Startup
{
    public static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic));
        
        return services;
    }
}