using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace RadzenBook.Infrastructure.Logger.Serilog;

public static class SerilogRegister
{
    public static WebApplicationBuilder UseSerilogging(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Environment.IsDevelopment()
            ? builder.Configuration.GetConnectionString("LocalConnection")
            : builder.Configuration.GetConnectionString("RemoteConnection");

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                path: "Logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.MSSqlServer(
                connectionString: connectionString,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = true,
                    SchemaName = "dbo",
                },
                restrictedToMinimumLevel: LogEventLevel.Error)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        
        builder.Host.UseSerilog();
        
        return builder;
    }
}