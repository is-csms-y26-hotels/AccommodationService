using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace AccommodationService.Infrastructure.Persistence.Migrations.BackgroundService;

public class MigrationBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public MigrationBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        IMigrationRunner runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}