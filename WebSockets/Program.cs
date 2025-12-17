using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();

//app.Map("/websocket", (websocketApp) =>
app.MapWhen(httpContext => httpContext.WebSockets.IsWebSocketRequest, websocketApp =>
{
    websocketApp.Run(async context =>
    {
        var websocket = await context.WebSockets.AcceptWebSocketAsync();

        var helloMessage = "Hello, WebSocket!";


        {
            var helloBuffer = System.Text.Encoding.UTF8.GetBytes(helloMessage);
            //await websocket.SendAsync(helloBuffer, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
            await websocket.SendAsync(new ArraySegment<byte>(helloBuffer, 0, 5), System.Net.WebSockets.WebSocketMessageType.Text, false, CancellationToken.None);
            await websocket.SendAsync(new ArraySegment<byte>(helloBuffer, 5, helloBuffer.Length - 5), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
        }

        _ = Task.Run(async () =>
        {
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(5000);
                if (!websocket.CloseStatus.HasValue && websocket.State == WebSocketState.Open)
                {
                    var ping = Encoding.UTF8.GetBytes("ping");
                    await websocket.SendAsync(ping, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        });

        var buffer = new byte[4];
        StringBuilder fullMessage = new StringBuilder();
        do
        {
            WebSocketReceiveResult receiveResult = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (receiveResult.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
            {
                await websocket.SendAsync(new ArraySegment<byte>(buffer, 0, receiveResult.Count), receiveResult.MessageType, receiveResult.EndOfMessage, CancellationToken.None);
            
                var @string = System.Text.Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                fullMessage.Append(@string);
                Debug.WriteLine($"Received: {@string}");
                if(receiveResult.EndOfMessage)
                {
                    Console.WriteLine($"Full message: {fullMessage.ToString()}");
                    fullMessage.Clear();
                }
            }
        } while(!websocket.CloseStatus.HasValue);

    });
});


app.MapGet("/", () => "Hello World!");

app.Run();
