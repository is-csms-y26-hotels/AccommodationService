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

    public override async Task<CreateRoomResponse> CreateRoom(
        CreateRoomRequest request,
        ServerCallContext context)
    {
        Application.Models.Rooms.RoomType mappedEnum = QueryMapper.MapFromGrpcEnum(request.RoomType);
        await _roomService.AddRoomAsync(request.HotelId, request.RoomNumber, mappedEnum, request.Price.DecimalValue, context.CancellationToken);

        return await Task.FromResult(new CreateRoomResponse());
    }

    public override async Task<UpdateRoomPriceResponse> UpdateRoomPrice(
        UpdateRoomPriceRequest request,
        ServerCallContext context)
    {
        await _roomService.UpdateRoomPriceAsync(request.RoomId, request.Price.DecimalValue, context.CancellationToken);

        return await Task.FromResult(new UpdateRoomPriceResponse());
    }

    public override async Task<SoftDeleteRoomResponse> SoftDeleteRoom(
        SoftDeleteRoomRequest request,
        ServerCallContext context)
    {
        await _roomService.SoftDeleteRoomAsync(request.RoomId, context.CancellationToken);

        return await Task.FromResult(new SoftDeleteRoomResponse());
    }

    public override async Task<ValidateRoomResponse> ValidateRoom(
        ValidateRoomRequest request,
        ServerCallContext context)
    {
        long? result = await _roomService.GetRoomIdAsync(request.RoomId, context.CancellationToken);

        return result is not null
            ? new ValidateRoomResponse
            {
                Result = true,
            }
            : new ValidateRoomResponse
            {
                Result = false,
            };
    }

    public override async Task<GetRoomPhysicalNumberResponse> GetRoomPhysicalNumber(
        GetRoomPhysicalNumberRequest request,
        ServerCallContext context)
    {
        long? result = await _roomService.GetPhysicalRoomNumberAsync(request.RoomId, context.CancellationToken);

        return result is not null
            ? new GetRoomPhysicalNumberResponse()
            {
                RoomNumber = (long)result,
            }
            : new GetRoomPhysicalNumberResponse();
    }

    public override async Task<GetHotelIdByRoomIdResponse> GetHotelIdByRoomId(
        GetHotelIdByRoomIdRequest request,
        ServerCallContext context)
    {
        long? hotelId = await _roomService.GetHotelIdByRoomIdAsync(request.RoomId, context.CancellationToken);

        return hotelId is not null
            ? new GetHotelIdByRoomIdResponse
            {
                HotelId = (long)hotelId,
            }
            : new GetHotelIdByRoomIdResponse();
    }
}