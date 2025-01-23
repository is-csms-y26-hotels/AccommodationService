namespace AccommodationService.Application.Models.Rooms;

public record Room(long RoomId, long HotelId, int RoomNumber, RoomType RoomType, decimal Price);