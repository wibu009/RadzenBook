using RadzenBook.API.Extensions;
using Serilog;

namespace RadzenBook.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.UseLogging();

                Log.Information("Starting application...");
                
                builder.Services.AddApplicationServices(builder.Configuration);
                builder.Services.AddIdentityServices(builder.Configuration);
                builder.Services.AddAuthServices(builder.Configuration);

                var app = builder.Build();
            
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseCustomMiddleware();
            
                app.UseSecurityHeaders();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();
            
                app.ApplyMigrations().Wait();
            
                app.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Application failed to start!!!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}