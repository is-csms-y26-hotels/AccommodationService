using AccommodationService.Application.Models.Rooms;

namespace AccommodationService.Application.Dtos;

public record RoomDto(long RoomId, long HotelId, int RoomNumber, RoomType RoomType, decimal Price);