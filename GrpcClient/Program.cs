
using Grpc.Core;
using Grpc.Net.Client;
using GrpcService.Protos.PeopleProto;
using GrpcService.Protos.StreamsProto;

using var channel = GrpcChannel.ForAddress("https://localhost:7074");

var streamClient = new GrpcService.Protos.StreamsProto.GrpcStream.GrpcStreamClient(channel);
var downStream = streamClient.FromServer(new Request { Message = "Hello from client" });

var token = new CancellationTokenSource();
//token.CancelAfter(10000);
token.CancelAfter(1000);
try
{
    while (await downStream.ResponseStream.MoveNext(token.Token))
    {
        Console.WriteLine(downStream.ResponseStream.Current.Message);
    }
}
catch (Exception e)
{
    Console.WriteLine("Streaming cancelled");
}
token.Dispose();


var upStream = streamClient.FromClient();
foreach (var letter in "ala ma kota")
{
    await upStream.RequestStream.WriteAsync(new Request { Message = letter.ToString() });
}
await upStream.RequestStream.CompleteAsync();
var response = await upStream.ResponseAsync;

Console.WriteLine(response.Message);



var streams = streamClient.Bidirectional();

_ = Task.Run(async () =>
{
    for (int i = 0; i < int.MaxValue; i++)
    {
        if (i % 2 == 0)
        {
            await streams.RequestStream.WriteAsync(new Request { Message = $"Message {i} from client" });
        }
        else
        {
            await streams.RequestStream.WriteAsync(new Request { Message = $"Inny komunikat {i} od klienta" });
        }
        await Task.Delay(750);
    }
});

_ = Task.Run(async () =>
{
    try
    {
        while (await streams.ResponseStream.MoveNext(CancellationToken.None))
        {
            Console.WriteLine(streams.ResponseStream.Current.Message);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Bidirectional streaming cancelled");
    }
});


Console.ReadLine();



static async Task Unary(GrpcChannel channel)
{
    var client = new GrpcService.Protos.PeopleProto.PeopleGrpcService.PeopleGrpcServiceClient(channel);

    var people = await client.ReadAsync(new Google.Protobuf.WellKnownTypes.Empty());

    foreach (var person in people.Collection)
    {
        Console.WriteLine($"Id: {person.Id.Value}, First Name: {person.FirstName}, Last Name: {person.LastName}");
    }
    Console.ReadLine();

    var p1 = await client.ReadByIdAsync(new Id { Value = 44 });
    p1 = await client.ReadByIdAsync(new Id { Value = 144 });

    var p2 = await client.ReadByIdOptionalAsync(new Id { Value = 44 });
    p2 = await client.ReadByIdOptionalAsync(new Id { Value = 144 });

    try
    {
        await client.UpdateAsync(new Person { Id = new Id { Value = 144 } });
    }
    catch (RpcException ex)
    {
        Console.WriteLine(ex.Status.Detail);
    }
}

