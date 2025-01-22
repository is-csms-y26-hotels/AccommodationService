using AccommodationService.Application.Models.Rooms;

namespace AccommodationService.Application.Dtos;

public record RoomDto(long RoomId, long HotelId, long RoomNumber, RoomType RoomType, decimal Price);