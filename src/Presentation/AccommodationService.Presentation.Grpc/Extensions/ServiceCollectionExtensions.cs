using AccommodationService.Presentation.Grpc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace AccommodationService.Presentation.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationGrpc(this IServiceCollection collection)
    {
        collection.AddGrpc();
        collection.AddGrpcReflection();
        collection.AddScoped<HotelController>();
        collection.AddScoped<RoomController>();
        return collection;
    }
}