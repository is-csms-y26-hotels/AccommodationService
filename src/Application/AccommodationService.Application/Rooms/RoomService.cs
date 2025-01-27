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
    private readonly IHotelRepository _hotelRepository;

    public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository)
    {
        _roomRepository = roomRepository;
        _hotelRepository = hotelRepository;
    }

    public async Task AddRoomAsync(long hotelId, long roomNumber, RoomType roomType, decimal price, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        long? result = await _hotelRepository.GetHotelByIdAsync(hotelId, cancellationToken);
        if (result is not null)
        {
            await _roomRepository.AddRoomAsync(hotelId, roomNumber, roomType, price, cancellationToken);
        }
        else
        {
            throw new ArgumentException("Hotel not found");
        }

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
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        IAsyncEnumerable<Room> rooms = _roomRepository.QueryAsync(query, cancellationToken);

        transaction.Complete();

        IAsyncEnumerable<RoomDto> roomsDtos = DtoMapper.MapToRoomDtos(rooms);
        IEnumerable<RoomDto> listRooms = roomsDtos.ToEnumerable();

        return await Task.FromResult(listRooms);
    }

    public async Task<long?> GetRoomIdAsync(long roomId, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        long? result = await _roomRepository.GetRoomByIdAsync(roomId, cancellationToken);

        transaction.Complete();

        return result;
    }

    public async Task<long?> GetPhysicalRoomNumberAsync(long roomId, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        long? result = await _roomRepository.GetRoomPhysicalNumberAsync(roomId, cancellationToken);

        transaction.Complete();

        return result;
    }
}