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

    public override async Task<CreateHotelResponse> CreateHotel(
        CreateHotelRequest request,
        ServerCallContext context)
    {
        await _hotelService.AddHotelAsync(request.HotelName, request.Stars, request.City, context.CancellationToken);

        return await Task.FromResult(new CreateHotelResponse());
    }

    public override async Task<UpdateHotelStarsResponse> UpdateHotelStars(
        UpdateHotelStarsRequest request,
        ServerCallContext context)
    {
        await _hotelService.UpdateHotelAsync(request.HotelId, request.Stars, context.CancellationToken);

        return await Task.FromResult(new UpdateHotelStarsResponse());
    }

    public override async Task<SoftDeleteHotelResponse> SoftDeleteHotel(
        SoftDeleteHotelRequest request,
        ServerCallContext context)
    {
        await _hotelService.SoftDeleteHotelAsync(request.HotelId, context.CancellationToken);

        return await Task.FromResult(new SoftDeleteHotelResponse());
    }

    public override async Task<ValidateHotelResponse> ValidateHotel(
        ValidateHotelRequest request,
        ServerCallContext context)
    {
        long? result = await _hotelService.GetByHotelIdAsync(request.HotelId, context.CancellationToken);

        return result is not null
            ? new ValidateHotelResponse
            {
                Result = true,
            }
            : new ValidateHotelResponse
            {
                Result = false,
            };
    }
}