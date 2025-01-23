using Accommodation.Service.Presentation.Grpc;
using AccommodationService.Application.Abstractions.Persistence.Queries;
using AccommodationService.Presentation.Grpc.Mappers;
using Grpc.Core;
using DomainRoomDto = AccommodationService.Application.Dtos.RoomDto;

namespace AccommodationService.Presentation.Grpc.Controllers;

public class RoomController : RoomService.RoomServiceBase
{
    private readonly Application.Rooms.RoomService _roomService;

    public RoomController(Application.Rooms.RoomService roomService)
    {
        _roomService = roomService;
    }

    public override async Task<GetRoomsByHotelResponse> GetRoomsByHotel(
        GetRoomsByHotelRequest request,
        ServerCallContext context)
    {
        RoomQuery query = QueryMapper.MapToRoomQuery(request);
        IEnumerable<DomainRoomDto> result = await _roomService.QueryAsync(query, context.CancellationToken);
        IEnumerable<RoomDto> grpcDto = QueryMapper.MapToGrpcRoomDto(result);

        var response = new GetRoomsByHotelResponse()
        {
            RoomsList = { grpcDto },
        };

        return await Task.FromResult(response);
    }
}