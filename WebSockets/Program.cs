using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.UseWebSockets();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
