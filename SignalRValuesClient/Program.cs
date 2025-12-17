
using Microsoft.AspNetCore.SignalR.Client;

var httpClient = new HttpClient();
var apiClient = new WebApi.ApiClient(httpClient);

var signalR = new HubConnectionBuilder()
    .WithUrl("https://localhost:5120/SignalR/values")
    .WithAutomaticReconnect()
    .Build();

var values = await apiClient.ValuesAllAsync();

foreach (var value in values)
{
    Console.WriteLine($"Value from API: {value}");
}

signalR.On<int>("ValueAdded", AddValue);
signalR.On<int>("ValueRemoved", RemoveValue);

await signalR.StartAsync();


Console.ReadLine();

foreach (var value in values)
{
    Console.WriteLine($"Value from API: {value}");
}

void AddValue(int value)
{
    values.Add(value);
    Console.WriteLine($"Added value: {value}");
}

void RemoveValue(int value)
{
    values.Remove(value);
    Console.WriteLine($"Removed value: {value}");
}