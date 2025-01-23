using SourceKit.Generators.Builder.Annotations;

namespace AccommodationService.Application.Abstractions.Persistence.Queries;

[GenerateBuilder]
public partial record HotelQuery(string City, int PageSize, long Cursor);