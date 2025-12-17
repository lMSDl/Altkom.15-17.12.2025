using HealthChecks.Checks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddSqlServer("Data Source=(local); Initial Catalog=dotnet;Integrated Security=true;TrustServerCertificate=true")
    .AddCheck(nameof(DirecotryAccessHealth), new DirecotryAccessHealth { DirectoryPath = "C:\\temp" });

builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI();

app.Run();
