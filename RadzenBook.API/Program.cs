using RadzenBook.API.Extensions;
using RadzenBook.API.Middlewares;

namespace RadzenBook.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddAuthServices(builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseSecurityHeaders();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            
            app.ApplyMigrations().Wait();
            
            app.Run();
        }
    }
}