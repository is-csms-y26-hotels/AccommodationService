using AccommodationService.Application.Abstractions.Persistence;
using AccommodationService.Application.Abstractions.Persistence.Repositories;
using AccommodationService.Application.Models.Rooms;
using AccommodationService.Infrastructure.Persistence.Migrations;
using AccommodationService.Infrastructure.Persistence.Options;
using AccommodationService.Infrastructure.Persistence.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace AccommodationService.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection collection)
    {
        collection.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(provider =>
                {
                    DatabaseOptions configuration = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
                    return configuration.ConnectionString;
                })
                .ScanIn(typeof(CreateEnums).Assembly).For.Migrations());

        collection.AddScoped<IRoomRepository, RoomRepository>();
        collection.AddScoped<IHotelRepository, HotelRepository>();
        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        collection.AddSingleton<NpgsqlDataSource>(provider =>
        {
            DatabaseOptions databaseSettings = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            string connectionString = databaseSettings.ConnectionString;

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            dataSourceBuilder.MapEnum<RoomType>();

            return dataSourceBuilder.Build();
        });
        return collection;
    }
}