using AccommodationService.Application.Abstractions.Persistence.Repositories;

namespace AccommodationService.Application.Abstractions.Persistence;

public interface IPersistenceContext
{
    IHotelRepository HotelRepository { get; }

    IRoomRepository RoomRepository { get; }
}