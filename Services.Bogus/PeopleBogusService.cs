using Bogus;
using Services.InMemory;

namespace Services.Bogus
{
    public class PeopleBogusService : PeopleService
    {
        public PeopleBogusService(Faker<Models.Person> faker, int count = 100) : base(faker.Generate(count))
        {
            for(int i = 1; i < count - 5; i++)
            {
                int parentIndex = Random.Shared.Next(0, i);
                var child = _items[i];
                var parent = _items[parentIndex];
                child.Parent = parent;
                parent.Children.Add(child);
            }

        }
    }
}
