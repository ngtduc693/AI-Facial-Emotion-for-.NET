using Example.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<EmotionDetectorService>();
app.MapGet("/", () => "gRPC service running...");

app.Run();
