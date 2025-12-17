using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService.Protos.PeopleProto;
using Services.Interfaces;
using Void = GrpcService.Protos.PeopleProto.Void;

namespace GrpcService.Services
{
    public class PeopleService : GrpcService.Protos.PeopleProto.PeopleGrpcService.PeopleGrpcServiceBase
    {
        private readonly IPeopleService _service;
        public PeopleService(IPeopleService service)
        {
            _service = service;
        }

        public override Task<Id> Create(Person request, ServerCallContext context)
        {

            return base.Create(request, context);
        }

        public override async Task<People> Read(Empty request, ServerCallContext context)
        {
            var people = await _service.ReadAsync();

            var response = new People();
            /*foreach (var person in people)
            {
                response.Collection.Add(new Person { Id = new Id { Value = person.Id }, FirstName = person.FirstName, LastName = person.LastName, Age = person.Age });
            }*/
            response.Collection.AddRange(people.Select(MapPerson));
            return response;
        }

        private static Person MapPerson(Models.Person person)
        {
            return new Person
            {
                Id = new Id { Value = person.Id },
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.Age
            };
        }

        public override async Task<Person> ReadById(Id request, ServerCallContext context)
        {
            var person = await _service.ReadByIdAsync(request.Value);
            if (person is null)
                return new Person();
            return MapPerson(person);
        }

        public override async Task<OptionalPerson> ReadByIdOptional(Id request, ServerCallContext context)
        {
            var person = await _service.ReadByIdAsync(request.Value);
            if (person is null)
                return new OptionalPerson { Empty = new Void() };
            return new OptionalPerson { Person = MapPerson(person) };
        }

        public override async Task<Void> Update(Person request, ServerCallContext context)
        {
            var person = await _service.ReadByIdAsync(request.Id.Value);

            if (person is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Person with Id={request.Id.Value} not found"));
            }

            person.FirstName = request.FirstName;
            person.LastName = request.LastName;
            person.Age = request.Age;
            await _service.UpdateAsync(request.Id.Value, person);
            return new Void();
        }

        public override async Task<Void> Delete(Id request, ServerCallContext context)
        {
            if (await _service.ReadByIdAsync(request.Value) is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Person with Id={request.Value} not found"));
            }

            await _service.DeleteAsync(request.Value);
            return new Void();
        }
    }
}
