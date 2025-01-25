using AccommodationService.Presentation.Grpc.Controllers;
using AccommodationService.Presentation.Grpc.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace AccommodationService.Presentation.Grpc.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationGrpc(this IServiceCollection collection)
    {
        collection.AddGrpc(grpc =>
        {
            grpc.Interceptors.Add<ServerInterceptor>();
        });
        collection.AddGrpcReflection();
        collection.AddScoped<HotelController>();
        collection.AddScoped<RoomController>();
        return collection;
    }
}