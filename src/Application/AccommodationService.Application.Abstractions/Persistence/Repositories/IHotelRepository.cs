using AccommodationService.Application.Abstractions.Persistence.Queries;
using AccommodationService.Application.Models.Hotels;

namespace AccommodationService.Application.Abstractions.Persistence.Repositories;

public interface IHotelRepository
{
    IAsyncEnumerable<Hotel> QueryAsync(HotelQuery query, CancellationToken cancellationToken);

    Task AddHotelAsync(string hotelName, int stars, string city, CancellationToken cancellationToken);

    Task UpdateHotelAsync(long hotelId, int stars, CancellationToken cancellationToken);

    Task SoftDeleteHotelAsync(long hotelId, CancellationToken cancellationToken);

    Task<long?> GetHotelByIdAsync(long hotelId, CancellationToken cancellationToken);

    Task<string> GetHotelNameByIdAsync(long hotelId, CancellationToken cancellationToken);
}