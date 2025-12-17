
using Grpc.Core;
using Grpc.Net.Client;
using GrpcService.Protos.PeopleProto;

using var channel = GrpcChannel.ForAddress("https://localhost:7074");
var client = new GrpcService.Protos.PeopleProto.PeopleGrpcService.PeopleGrpcServiceClient(channel);

var people = await client.ReadAsync(new Google.Protobuf.WellKnownTypes.Empty());

foreach (var person in people.Collection)
{
    Console.WriteLine( $"Id: {person.Id.Value}, First Name: {person.FirstName}, Last Name: {person.LastName}" );
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