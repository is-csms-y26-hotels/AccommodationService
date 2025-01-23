using Accommodation.Service.Presentation.Grpc;
using AccommodationService.Application.Abstractions.Persistence.Queries;
using DomainHotelDto = AccommodationService.Application.Dtos.HotelDto;
using DomainRoomDto = AccommodationService.Application.Dtos.RoomDto;
using DomainRoomType = AccommodationService.Application.Models.Rooms.RoomType;

namespace AccommodationService.Presentation.Grpc.Mappers;

public static class QueryMapper
{
    public static RoomQuery MapToRoomQuery(GetRoomsByHotelRequest request)
    {
        DomainRoomType mappedEnum = MapFromGrpcEnum(request.RoomType);

        return new RoomQuery(
            HotelId: request.HotelId,
            RoomType: mappedEnum,
            PageSize: request.PageSize,
            Cursor: request.Cursor);
    }

    public static IEnumerable<RoomDto> MapToGrpcRoomDto(IEnumerable<DomainRoomDto> domainRoomDtos)
    {
        return new List<RoomDto>(
            domainRoomDtos.Select(dto => new RoomDto
                {
                    HotelId = dto.HotelId,
                    RoomType = MapToGrpcEnum(dto.RoomType),
                    Price = MapMoneyType(dto.Price),
                    RoomNumber = dto.RoomNumber,
                    RoomId = dto.RoomId,
                })
                .ToList());
    }

    public static IEnumerable<HotelDto> MapToGrpcHotelDto(IEnumerable<DomainHotelDto> domainHotelDtos)
    {
        return new List<HotelDto>(
            domainHotelDtos.Select(dto => new HotelDto
                {
                    HotelId = dto.HotelId,
                    Name = dto.Name,
                    Stars = dto.Stars,
                    City = dto.City,
                })
                .ToList());
    }

    private static DomainRoomType MapFromGrpcEnum(RoomType roomType)
    {
        return roomType switch
        {
            RoomType.Unspecified => throw new NotImplementedException(),
            RoomType.Standard => DomainRoomType.Standard,
            RoomType.Twin => DomainRoomType.Twin,
            RoomType.Studio => DomainRoomType.Studio,
            RoomType.JuniorSuite => DomainRoomType.JuniorSuite,
            RoomType.Deluxe => DomainRoomType.Deluxe,
            _ => throw new InvalidOperationException(),
        };
    }

    private static RoomType MapToGrpcEnum(DomainRoomType roomType)
    {
        return roomType switch
        {
            DomainRoomType.Standard => RoomType.Standard,
            DomainRoomType.Twin => RoomType.Twin,
            DomainRoomType.Studio => RoomType.Studio,
            DomainRoomType.JuniorSuite => RoomType.JuniorSuite,
            DomainRoomType.Deluxe => RoomType.Deluxe,
            _ => throw new InvalidOperationException(),
        };
    }

    private static Google.Type.Money MapMoneyType(decimal money)
    {
        var mappedMoney = new Google.Type.Money
        {
            DecimalValue = money,
        };

        return mappedMoney;
    }
}