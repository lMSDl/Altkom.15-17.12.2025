using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class DemoHub : Hub
    {
        public DemoHub() {
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            Console.WriteLine(Context.ConnectionId);
            await Clients.Caller.SendAsync("HelloMessage", "Welcome to the DemoHub! Your connection ID is: " + Context.ConnectionId);
            await Clients.Others.SendAsync("UserConnected", $"A new user has connected: {Context.ConnectionId}");
        }


        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task JoinToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.OthersInGroup(groupName).SendAsync("UserConnected", $"User {Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task SendDemo()
        {
            await Clients.All.SendAsync("ReceiveRecord", new DemoRecord(Random.Shared.Next().ToString(), Random.Shared.Next()));
            await Clients.All.SendAsync("ReceiveRecord", Random.Shared.Next().ToString(), Random.Shared.Next());
        }
    }

    record DemoRecord(string Name, int Value);
}
