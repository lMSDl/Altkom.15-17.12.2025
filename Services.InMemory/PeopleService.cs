using Models;

namespace Services.InMemory
{
    public class PeopleService : GenericService<Person>, Interfaces.IPeopleService
    {
        public Task<IEnumerable<Person>> ReadByName(string name)
        {
            var people = _items.Where(x => x.FirstName.Contains(name, StringComparison.InvariantCultureIgnoreCase)
            || x.LastName.Contains(name, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult(people);
        }
    }
}
