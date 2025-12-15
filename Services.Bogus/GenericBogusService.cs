using Bogus;
using Services.InMemory;

namespace Services.Bogus
{
    public class GenericBogusService<T> : GenericService<T> where T : Models.Entity
    {
        public GenericBogusService(Faker<T> faker, int count = 100) : base(faker.Generate(count))
        {
        }
    }
}
