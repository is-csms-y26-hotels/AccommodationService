using AccommodationService.Application.Abstractions.Persistence;
using AccommodationService.Application.Abstractions.Persistence.Repositories;

namespace AccommodationService.Infrastructure.Persistence;

public class PersistenceContext : IPersistenceContext
{
    public IHotelRepository HotelRepository { get; }

    public IRoomRepository RoomRepository { get; }

    public PersistenceContext(IRoomRepository roomRepository, IHotelRepository hotelRepository)
    {
        HotelRepository = hotelRepository;
        RoomRepository = roomRepository;
    }
}