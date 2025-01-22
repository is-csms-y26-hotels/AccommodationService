using AccommodationService.Application.Dtos;
using AccommodationService.Application.Models.Hotels;
using AccommodationService.Application.Models.Rooms;

namespace AccommodationService.Application.Mappers;

public static class DtoMapper
{
    public static async IAsyncEnumerable<RoomDto> MapToRoomDtos(IAsyncEnumerable<Room> rooms)
    {
        IList<RoomDto> dtos = new List<RoomDto>();
        await foreach (Room room in rooms)
        {
            yield return new RoomDto(
                RoomId: room.RoomId,
                HotelId: room.HotelId,
                RoomNumber: room.RoomNumber,
                RoomType: room.RoomType,
                Price: room.Price);
        }
    }

    public static async IAsyncEnumerable<HotelDto> MapToHotelDtos(IAsyncEnumerable<Hotel> hotels)
    {
        IList<RoomDto> dtos = new List<RoomDto>();
        await foreach (Hotel hotel in hotels)
        {
            yield return new HotelDto(
                HotelId: hotel.HotelId,
                Name: hotel.Name,
                Stars: hotel.Stars,
                City: hotel.City);
        }
    }
}