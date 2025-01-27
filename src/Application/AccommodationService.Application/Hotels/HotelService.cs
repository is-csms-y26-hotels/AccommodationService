using AccommodationService.Application.Abstractions.Persistence.Queries;
using AccommodationService.Application.Abstractions.Persistence.Repositories;
using AccommodationService.Application.Dtos;
using AccommodationService.Application.Mappers;
using AccommodationService.Application.Models.Hotels;
using System.Transactions;

namespace AccommodationService.Application.Hotels;

public class HotelService
{
    private readonly IHotelRepository _hotelRepository;

    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task AddHotelAsync(string hotelName, int stars, string city, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        await _hotelRepository.AddHotelAsync(hotelName, stars, city, cancellationToken);

        transaction.Complete();
    }

    public async Task UpdateHotelAsync(long hotelId, int stars, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        await _hotelRepository.UpdateHotelAsync(hotelId, stars, cancellationToken);

        transaction.Complete();
    }

    public async Task SoftDeleteHotelAsync(long hotelId, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        await _hotelRepository.SoftDeleteHotelAsync(hotelId, cancellationToken);

        transaction.Complete();
    }

    public async Task<IEnumerable<HotelDto>> QueryAsync(HotelQuery query, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        IAsyncEnumerable<Hotel> hotels = _hotelRepository.QueryAsync(query, cancellationToken);

        transaction.Complete();

        IAsyncEnumerable<HotelDto> hotelsDtos = DtoMapper.MapToHotelDtos(hotels);
        IEnumerable<HotelDto> listHotels = hotelsDtos.ToEnumerable();

        return await Task.FromResult(listHotels);
    }

    public async Task<long?> GetByHotelIdAsync(long hotelId, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        long? result = await _hotelRepository.GetHotelByIdAsync(hotelId, cancellationToken);

        transaction.Complete();

        return result;
    }

    public async Task<string> GetHotelNameByIdAsync(long hotelId, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        string hotelName = await _hotelRepository.GetHotelNameByIdAsync(hotelId, cancellationToken);

        transaction.Complete();

        return hotelName;
    }
}
