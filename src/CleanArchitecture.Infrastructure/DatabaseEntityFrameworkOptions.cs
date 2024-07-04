namespace CleanArchitecture.Infrastructure;

public class DatabaseEntityFrameworkOptions
{
    public const string SectionName = "DatabaseEntityFrameworkOptions";
    public string ConnectionString { get; set; }
    public int MaxRetryCount { get; set; }
    public int CommandTimeout { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
}
