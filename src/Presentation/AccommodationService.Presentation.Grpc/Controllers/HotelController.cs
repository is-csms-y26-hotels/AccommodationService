using Accommodation.Service.Presentation.Grpc;
using AccommodationService.Application.Abstractions.Persistence.Queries;
using AccommodationService.Presentation.Grpc.Mappers;
using Grpc.Core;
using DomainHotelDto = AccommodationService.Application.Dtos.HotelDto;

namespace AccommodationService.Presentation.Grpc.Controllers;

public class HotelController : HotelService.HotelServiceBase
{
    private readonly Application.Hotels.HotelService _hotelService;

    public HotelController(Application.Hotels.HotelService hotelService)
    {
        _hotelService = hotelService;
    }

    public override async Task<GetHotelsResponse> GetHotels(
        GetHotelsRequest request,
        ServerCallContext context)
    {
        var query = new HotelQuery(
            City: request.City,
            PageSize: request.PageSize,
            Cursor: request.Cursor);

        IEnumerable<DomainHotelDto> result = await _hotelService.QueryAsync(query, context.CancellationToken);
        IEnumerable<HotelDto> grpcDto = QueryMapper.MapToGrpcHotelDto(result);

        var response = new GetHotelsResponse()
        {
            HotelList = { grpcDto },
        };

        return await Task.FromResult(response);
    }
}