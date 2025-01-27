using AccommodationService.Application.Abstractions.Persistence.Queries;
using AccommodationService.Application.Abstractions.Persistence.Repositories;
using AccommodationService.Application.Models.Hotels;
using Npgsql;
using System.Runtime.CompilerServices;

namespace AccommodationService.Infrastructure.Persistence.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly NpgsqlDataSource _npgsqlDataSource;

    public HotelRepository(NpgsqlDataSource npgsqlDataSource)
    {
        _npgsqlDataSource = npgsqlDataSource;
    }

    public async IAsyncEnumerable<Hotel> QueryAsync(HotelQuery query, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
        SELECT hotel_id, hotel_name, stars, city, hotel_deleted
        FROM hotels
        WHERE (hotel_id >= @cursor)
        AND (@city IS NULL OR city = @city)
        ORDER BY hotel_id
        LIMIT @pageSize 
        """;

        await using NpgsqlConnection connection = await _npgsqlDataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("cursor", query.Cursor));
        command.Parameters.Add(new NpgsqlParameter("pageSize", query.PageSize));
        command.Parameters.Add(new NpgsqlParameter("city", query.City));

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new Hotel(
                HotelId: reader.GetInt64(reader.GetOrdinal("hotel_id")),
                Name: reader.GetString(reader.GetOrdinal("hotel_name")),
                Stars: reader.GetInt32(reader.GetOrdinal("stars")),
                City: reader.GetString(reader.GetOrdinal("city")));
        }
    }

    public async Task AddHotelAsync(string hotelName, int stars, string city, CancellationToken cancellationToken)
    {
        const string sql = """
        INSERT INTO hotels(hotel_name, stars, city, hotel_deleted)
        VALUES (@hotelName, @stars, @city, false)
        """;

        await using NpgsqlConnection connection = await _npgsqlDataSource.OpenConnectionAsync(cancellationToken);

        var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@hotelName", hotelName);
        command.Parameters.AddWithValue("@stars", stars);
        command.Parameters.AddWithValue("@city", city);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task UpdateHotelAsync(long hotelId, int stars, CancellationToken cancellationToken)
    {
        const string sql = """
        UPDATE hotels SET stars = @stars
        WHERE hotel_id = @hotelId";
        """;

        await using NpgsqlConnection connection = await _npgsqlDataSource.OpenConnectionAsync(cancellationToken);

        var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@hotelId", hotelId);
        command.Parameters.AddWithValue("@stars", stars);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task SoftDeleteHotelAsync(long hotelId, CancellationToken cancellationToken)
    {
        const string sql = """
        UPDATE hotels SET hotel_deleted = true
        WHERE hotel_id = @hotelId"
        """;

        await using NpgsqlConnection connection = await _npgsqlDataSource.OpenConnectionAsync(cancellationToken);

        var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@hotelId", hotelId);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<long?> GetHotelByIdAsync(long hotelId, CancellationToken cancellationToken)
    {
        const string sql = """
        SELECT hotel_id
        FROM hotels
        WHERE hotel_id = @hotelId
        """;

        await using NpgsqlConnection connection = await _npgsqlDataSource.OpenConnectionAsync(cancellationToken);

        var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@hotelId", hotelId);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        long? result = null;
        while (await reader.ReadAsync(cancellationToken))
        {
             result = reader.GetInt64(reader.GetOrdinal("hotel_id"));
             return result;
        }

        return result;
    }

    public async Task<string> GetHotelNameByIdAsync(long hotelId, CancellationToken cancellationToken)
    {
        const string sql = """
        SELECT hotel_name
        FROM hotels
        WHERE hotel_id = @hotelId";
        """;

        await using NpgsqlConnection connection = await _npgsqlDataSource.OpenConnectionAsync(cancellationToken);

        var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@hotelId", hotelId);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        string result = string.Empty;
        while (await reader.ReadAsync(cancellationToken))
        {
            result = reader.GetString(reader.GetOrdinal("hotel_name"));
            return result;
        }

        return result;
    }
}