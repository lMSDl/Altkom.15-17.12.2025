using Bogus;
using Models;

namespace Services.Bogus.Fakers
{
    public abstract class EntityFaker<T> : Faker<T> where T : Entity
    {
        public EntityFaker() : base("pl")
        {
            RuleFor(x => x.Id, x => x.IndexFaker + 1);
        }
    }
}
