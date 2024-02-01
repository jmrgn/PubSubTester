using Google.Api.Gax;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using PubSubTester;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var serializerOptions = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
};

app.MapPost("api/messages", async (AddMessageRequest request) =>
{
    Environment.SetEnvironmentVariable("PUBSUB_EMULATOR_HOST", request.EmulatorPort);
    var publisher = await new PublisherClientBuilder
    {
        TopicName = TopicName.FromProjectTopic(request.ProjectId, request.TopicName),
        EmulatorDetection = EmulatorDetection.EmulatorOnly
    }.BuildAsync();

    var message = ByteString.CopyFromUtf8(JsonSerializer.Serialize(
                    request.Data, serializerOptions));

    await publisher.PublishAsync(message);
    await publisher.ShutdownAsync(TimeSpan.FromSeconds(10));

    return Results.Ok();
})
.WithOpenApi();

app.Run();
