using SourceKit.Generators.Builder.Annotations;

namespace AccommodationService.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public partial record RoomQuery(long[] RoomIds, int PageSize, long Cursor);