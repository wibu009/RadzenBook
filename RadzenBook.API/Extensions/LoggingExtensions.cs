using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace RadzenBook.API.Extensions;

public static class LoggingExtensions
{
    public static WebApplicationBuilder UseLogging(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Environment.IsDevelopment()
                ? builder.Configuration.GetConnectionString("LocalConnection")
                : builder.Configuration.GetConnectionString("RemoteConnection");

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.MSSqlServer(
                connectionString,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = true
                })
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        
        builder.Host.UseSerilog();
        
        return builder;
    }
}