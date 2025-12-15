using Services.Interfaces;

namespace Services.InMemory
{
    public class GenericService<T> : IGenericService<T> where T : Models.Entity
    {
        protected readonly List<T> _items = new();


        public Task<int> CreateAsync(T entity)
        {
            entity.Id = _items.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _items.Add(entity);
            return Task.FromResult(entity.Id);
        }

        public Task DeleteAsync(int id)
        {
            var entity = _items.FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                _items.Remove(entity);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            return Task.FromResult<IEnumerable<T>>([.. _items]);
        }

        public Task<IEnumerable<T>> ReadAsync(Func<T, bool> func)
        {
            var result = _items.Where(func).ToArray();
            return Task.FromResult<IEnumerable<T>>(result);
        }

        public Task<T?> ReadByIdAsync(int id)
        {
            var entity = _items.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(entity);
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var existingEntity = _items.FirstOrDefault(e => e.Id == id);
            if (existingEntity != null)
            {
                await DeleteAsync(id);
                entity.Id = id;
                _items.Add(entity);
            }
        }
    }
}
