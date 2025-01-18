namespace AccommodationService.Application.Models.Rooms;

public record Room(long RoomId, long HotelId, long RoomNumber, RoomType RoomType, decimal Price);