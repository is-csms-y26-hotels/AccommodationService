using AccommodationService.Application.Abstractions.Persistence.Queries;
using AccommodationService.Application.Models.Rooms;

namespace AccommodationService.Application.Abstractions.Persistence.Repositories;

public interface IRoomRepository
{
    IAsyncEnumerable<Room> QueryAsync(RoomQuery query, CancellationToken cancellationToken);

    Task AddRoomAsync(long hotelId, long roomNumber, RoomType roomType, decimal price, CancellationToken cancellationToken);

    Task UpdateRoomPriceAsync(long roomId,  decimal price, CancellationToken cancellationToken);

    Task SoftDeleteRoomAsync(long roomId, CancellationToken cancellationToken);
}