using Microsoft.Extensions.Hosting;
using RadzenBook.Infrastructure.Persistence.Configurations;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace RadzenBook.Infrastructure.Logger.Serilog;

public static class SerilogRegister
{
    public static WebApplicationBuilder UseSerilogging(this WebApplicationBuilder builder)
    {
        var proEnv = Environment.GetEnvironmentVariable("PROVIDER_ENVIRONMENT");

        var connectionString = proEnv switch
        {
            "Local" => builder.Configuration.GetConnectionString("LocalConnection"),
            "Server" => builder.Configuration.GetConnectionString("RemoteConnection"),
            "Docker" => builder.Configuration.GetConnectionString("DockerConnection"),
            _ => throw new NullReferenceException("Connection string is missing")
        };

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
                    SchemaName = SchemaName.Default
                },
                restrictedToMinimumLevel: LogEventLevel.Error)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        builder.Host.UseSerilog();

        return builder;
    }
}