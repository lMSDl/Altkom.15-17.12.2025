using Bogus;
using Services.InMemory;

namespace Services.Bogus
{
    public class PeopleBogusService : PeopleService
    {
        public PeopleBogusService(Faker<Models.Person> faker, int count = 100) : base(faker.Generate(count))
        {
        }
    }
}
