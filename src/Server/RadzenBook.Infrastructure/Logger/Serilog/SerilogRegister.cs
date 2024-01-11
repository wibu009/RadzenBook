using RadzenBook.Infrastructure.Persistence;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace RadzenBook.Infrastructure.Logger.Serilog;

public static class SerilogRegister
{
    public static WebApplicationBuilder UseSerilogging(this WebApplicationBuilder builder)
    {
        var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>()!;
        var connectionString = databaseSettings.ConnectionString;
        var databaseProvider = databaseSettings.DatabaseProvider;

        var loggerConfiguration = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                path: "Logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] {Message:lj}{NewLine}{Exception}"
            );

        switch (databaseProvider)
        {
            case "SqlServer":
                loggerConfiguration.WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true
                    });
                break;
            case "PostgreSql":
                loggerConfiguration.WriteTo.PostgreSQL(
                    connectionString: connectionString,
                    tableName: "Logs",
                    needAutoCreateTable: true);
                break;
            case "Sqlite":
                loggerConfiguration.WriteTo.SQLite(
                    tableName: "Logs",
                    sqliteDbPath: connectionString,
                    batchSize: 1);
                break;
            case "MySql":
                loggerConfiguration.WriteTo.MySQL(
                    connectionString: connectionString,
                    tableName: "Logs");
                break;
            default:
                throw new NullReferenceException("Database provider is missing");
        }

        Log.Logger = loggerConfiguration.CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        builder.Host.UseSerilog();

        return builder;
    }
}