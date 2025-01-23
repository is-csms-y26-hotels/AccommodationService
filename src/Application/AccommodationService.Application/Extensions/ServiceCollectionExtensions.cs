using AccommodationService.Application.Hotels;
using AccommodationService.Application.Rooms;
using Microsoft.Extensions.DependencyInjection;

namespace AccommodationService.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<HotelService>();
        collection.AddScoped<RoomService>();
        return collection;
    }
}