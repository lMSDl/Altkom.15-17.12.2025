using Microsoft.AspNetCore.SignalR.Client;


var signalR = new HubConnectionBuilder()
    .WithUrl("https://localhost:7178/SignalR/demo")
    .Build();

signalR.On<string>("HelloMessage", x => HelloMessage(x));
signalR.On<string>(nameof(UserConnected), UserConnected);
signalR.On<string>(nameof(ReceiveMessage), ReceiveMessage);
//signalR.On<DemoClass>(nameof(ReceiveRecord), ReceiveRecord);
signalR.On<string, int>(nameof(ReceiveRecord), ReceiveRecord);


await signalR.StartAsync();


await signalR.SendAsync("SendMessageToAll", "Hello from SignalR Client!");
await signalR.SendAsync("JoinToGroup", "alamakota");
await signalR.SendAsync("SendDemo");

Console.ReadLine();


void HelloMessage(string message)
{
    Console.WriteLine("Message from server: " + message);
}

void UserConnected(string message)
{
    Console.WriteLine("Notification: " + message);
}

void ReceiveMessage(string message)
{
       Console.WriteLine("Broadcast message: " + message);
}

/*void ReceiveRecord(DemoClass record)
{
    Console.WriteLine($"Received record: Name = {record.Name}, Value = {record.Value}");
}*/

void ReceiveRecord(string Name, int Value)
{
    Console.WriteLine($"Received record: Name = {Name}, Value = {Value}");
}

class DemoClass()
{
    public string Name { get; set; }
    public int Value { get; set; }
}