using AccommodationService.Application.Abstractions.Persistence.Queries;
using AccommodationService.Application.Abstractions.Persistence.Repositories;
using AccommodationService.Application.Dtos;
using AccommodationService.Application.Mappers;
using AccommodationService.Application.Models.Rooms;
using System.Transactions;

namespace AccommodationService.Application.Rooms;

public class RoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task AddRoomAsync(long hotelId, long roomNumber, RoomType roomType, decimal price, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        await _roomRepository.AddRoomAsync(hotelId, roomNumber, roomType, price, cancellationToken);

        transaction.Complete();
    }

    public async Task UpdateRoomPriceAsync(long roomId, decimal price, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        await _roomRepository.UpdateRoomPriceAsync(roomId, price, cancellationToken);

        transaction.Complete();
    }

    public async Task SoftDeleteRoomAsync(long roomId, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        await _roomRepository.SoftDeleteRoomAsync(roomId, cancellationToken);

        transaction.Complete();
    }

    public async Task<IEnumerable<RoomDto>> QueryAsync(RoomQuery query, CancellationToken cancellationToken)
    {
        IAsyncEnumerable<Room> rooms = _roomRepository.QueryAsync(query, cancellationToken);

        IAsyncEnumerable<RoomDto> roomsDtos = DtoMapper.MapToRoomDtos(rooms);
        IEnumerable<RoomDto> listRooms = roomsDtos.ToEnumerable();

        return await Task.FromResult(listRooms);
    }
}