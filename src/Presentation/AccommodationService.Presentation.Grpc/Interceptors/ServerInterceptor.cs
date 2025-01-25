using Grpc.Core;
using Grpc.Core.Interceptors;

namespace AccommodationService.Presentation.Grpc.Interceptors;

public class ServerInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            if (ex.Message is not null && ex.Message.EndsWith("found"))
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }

            throw;
        }
    }
}