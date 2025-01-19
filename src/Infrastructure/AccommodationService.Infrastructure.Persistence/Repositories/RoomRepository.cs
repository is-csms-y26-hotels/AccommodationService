using AccommodationService.Application.Abstractions.Persistence.Queries;
using AccommodationService.Application.Abstractions.Persistence.Repositories;
using AccommodationService.Application.Models.Rooms;
using Npgsql;
using System.Runtime.CompilerServices;

namespace AccommodationService.Infrastructure.Persistence.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly NpgsqlDataSource _npgsqlDataSource;

    public RoomRepository(NpgsqlDataSource npgsqlDataSource)
    {
        _npgsqlDataSource = npgsqlDataSource;
    }

    public async IAsyncEnumerable<Room> QueryAsync(RoomQuery query, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
        SELECT room_id, hotel_id, room_number, room_type, room_price, room_deleted
        FROM rooms
        WHERE (cardinaluty(@ids) = 0 OR room_id = any(@ids))
        AND (room_id > @cursor)
        AND (@type IS NULL OR room_type = @type)
        ORDER BY room_id
        LIMIT @pageSize
        """;

        await using NpgsqlConnection connection = await _npgsqlDataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("cursor", query.Cursor));
        command.Parameters.Add(new NpgsqlParameter("ids", query.RoomIds));
        command.Parameters.Add(new NpgsqlParameter("pageSize", query.PageSize));
        command.Parameters.Add(new NpgsqlParameter("type", query.RoomType));

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new Room(
                RoomId: reader.GetInt64(reader.GetOrdinal("room_id")),
                HotelId: reader.GetInt64(reader.GetOrdinal("hotel_id")),
                RoomNumber: reader.GetInt64(reader.GetOrdinal("room_number")),
                RoomType: (RoomType)reader.GetValue(reader.GetOrdinal("room_type")),
                Price: reader.GetDecimal(reader.GetOrdinal("room_price")));
        }
    }

    public async Task AddRoomAsync(long hotelId, long roomNumber, RoomType roomType, decimal price, CancellationToken cancellationToken)
    {
        const string sql = """
        INSERT INTO rooms(hotel_id, room_number, room_type, room_price, room_deleted)
        VALUES (@hotelId, @roomNumber, @roomType, @roomPrice, false)
        """;

        await using NpgsqlConnection connection = await _npgsqlDataSource.OpenConnectionAsync(cancellationToken);

        var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@hotelId", hotelId);
        command.Parameters.AddWithValue("@roomNumber", roomNumber);
        command.Parameters.AddWithValue("@roomType", roomType);
        command.Parameters.AddWithValue("@roomPrice", price);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task UpdateRoomPriceAsync(long roomId, decimal price, CancellationToken cancellationToken)
    {
        const string sql = """
        UPDATE rooms SET room_price = @roomPrice
        WHERE roomId = @roomId
        """;

        await using NpgsqlConnection connection = await _npgsqlDataSource.OpenConnectionAsync(cancellationToken);

        var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@roomPrice", price);
        command.Parameters.AddWithValue("@roomId", roomId);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task SoftDeleteRoomAsync(long roomId, CancellationToken cancellationToken)
    {
        const string sql = """
        UPDATE rooms SET room_deleted = true
        WHERE roomId = @roomId
        """;

        await using NpgsqlConnection connection = await _npgsqlDataSource.OpenConnectionAsync(cancellationToken);

        var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@roomId", roomId);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}