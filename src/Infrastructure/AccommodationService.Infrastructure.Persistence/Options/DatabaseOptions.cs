namespace AccommodationService.Infrastructure.Persistence.Options;

public class DatabaseOptions
{
    public string Host { get; set; } = string.Empty;

    public string Port { get; set; } = string.Empty;

    public string Database { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConnectionString =>
        $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
}