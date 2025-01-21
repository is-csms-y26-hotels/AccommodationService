using AccommodationService.Application.Models.Rooms;
using SourceKit.Generators.Builder.Annotations;

namespace AccommodationService.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public partial record RoomQuery(long HotelId, RoomType? RoomType, int PageSize, long Cursor);