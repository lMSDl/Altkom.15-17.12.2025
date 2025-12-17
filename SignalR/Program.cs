using SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapHub<DemoHub>("SignalR/Demo");

app.Run();
