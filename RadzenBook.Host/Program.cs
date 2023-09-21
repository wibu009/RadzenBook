using RadzenBook.Application;
using RadzenBook.Infrastructure;
using RadzenBook.Infrastructure.Logger;
using Serilog;

namespace RadzenBook.Host;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.UseLogging();
                
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            var app = builder.Build();
                
            app.UseInfrastructure();
            
            app.Run();
        }
        catch (Exception ex)
        {
            if (ex.GetType().Name.Equals("StopTheHostException")) throw;
            Log.Fatal(ex, "Application failed to start!!!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}