namespace RadzenBook.Infrastructure.Persistence;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = default!;
    public string DatabaseProvider { get; set; } = default!;
    public string DatabaseEnvironment { get; set; } = default!;
}