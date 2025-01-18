using SourceKit.Generators.Builder.Annotations;

namespace AccommodationService.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public partial record HotelQuery(long[] HotelIds, int PageSize, long Cursor);