using AccommodationService.Infrastructure.Persistence.Options;
using Microsoft.Extensions.DependencyInjection;

namespace AccommodationService.Infrastructure.Persistence.Extensions;

public static class OptionsExtension
{
    public static IServiceCollection AddAllOptions(this IServiceCollection collection)
    {
        collection.AddOptions<DatabaseOptions>().BindConfiguration("Infrastructure:Persistence:Postgres");
        return collection;
    }
}