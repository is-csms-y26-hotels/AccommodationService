using AccommodationService.Application.Abstractions.Persistence;
using AccommodationService.Application.Abstractions.Persistence.Repositories;
using AccommodationService.Application.Models.Rooms;
using AccommodationService.Infrastructure.Persistence.Migrations;
using AccommodationService.Infrastructure.Persistence.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace AccommodationService.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection collection)
    {
        // TODO: опшены для дб
        collection.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString("Host=localhost;Port=5433;Database=postgres;Username=postgres;Password=postgres;")
                .ScanIn(typeof(CreateEnums).Assembly).For.Migrations());

        collection.AddScoped<IRoomRepository, RoomRepository>();
        collection.AddScoped<IHotelRepository, HotelRepository>();
        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        collection.AddSingleton<NpgsqlDataSource>(provider =>
        {
            // DatabaseConfiguration databaseSettings = provider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;
            string connectionString = "Host=localhost;Port=5433;Database=postgres;Username=postgres;Password=postgres;";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            dataSourceBuilder.MapEnum<RoomType>();

            return dataSourceBuilder.Build();
        });
        return collection;
    }
}