#pragma warning disable CA1506

using Itmo.Dev.Platform.Common.Extensions;
#pragma warning disable SA1210
using Itmo.Dev.Platform.Observability;
using Itmo.Dev.Platform.Events;
#pragma warning restore SA1210
using AccommodationService.Application.Extensions;
using AccommodationService.Infrastructure.Persistence.Extensions;
using AccommodationService.Infrastructure.Persistence.Migrations.BackgroundService;
using AccommodationService.Presentation.Grpc.Controllers;
using AccommodationService.Presentation.Grpc.Extensions;
using AccommodationService.Presentation.Kafka.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddOptions<JsonSerializerSettings>();
builder.Services.AddAllOptions();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JsonSerializerSettings>>().Value);

builder.Services.AddPlatform();
builder.AddPlatformObservability();

builder.Services.AddApplication();
builder.Services.AddInfrastructurePersistence();
builder.Services.AddPresentationGrpc();
builder.Services.AddPresentationKafka(builder.Configuration);

builder.Services.AddPlatformEvents(b => b.AddPresentationKafkaHandlers());

builder.Services.AddUtcDateTimeProvider();

builder.Services.AddHostedService<MigrationBackgroundService>();

WebApplication app = builder.Build();

app.UseRouting();

app.UsePlatformObservability();

app.UsePresentationGrpc();

app.MapGrpcService<HotelController>();
app.MapGrpcService<RoomController>();

await app.RunAsync();