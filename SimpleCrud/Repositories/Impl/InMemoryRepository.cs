using SimpleCrud.Entities;
using System.Collections.Concurrent;

namespace SimpleCrud.Repositories.Impl
{
    public class InMemoryRepository : IRepository
    {
        // Store per-type dictionaries of entities keyed by Id, using ConcurrentDictionary for thread-safety
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<int, IEntity>> _store = new();

        // Per-type id counters
        private readonly ConcurrentDictionary<Type, int> _counters = new();

        public Task AddAsync<T>(T entity) where T : IEntity
        {
            var type = typeof(T);
            var dict = _store.GetOrAdd(type, _ => new ConcurrentDictionary<int, IEntity>());

            // Atomically get a new id for this type
            var newId = _counters.AddOrUpdate(type, 1, (t, current) => current + 1);
            entity.Id = newId;
            entity.CreatedDate = DateTime.Now;

            if (!dict.TryAdd(entity.Id, entity))
            {
                // using exception to simulate a database failure
                throw new InvalidOperationException($"An entity of type {type.Name} with Id {entity.Id} already exists.");
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync<T>(T entity) where T : IEntity
        {
            if (entity.Id <= 0)
                throw new ArgumentException("Entity must have a valid Id to be updated.", nameof(entity));

            var type = typeof(T);
            var dict = _store.GetOrAdd(type, _ => new ConcurrentDictionary<int, IEntity>());

            // Replace existing or add if missing
            dict.AddOrUpdate(entity.Id, entity, (id, existing) =>
            {
                entity.CreatedDate = existing.CreatedDate; // preserve CreatedDate
                return entity;
            });

            return Task.CompletedTask;
        }

        public Task<int> DeleteAsync<T>(T entity) where T : IEntity
        {
            // delete should not throw error if entity does not exists or is null

            if (entity is null) return Task.FromResult(0);

            var type = typeof(T);

            if (!_store.TryGetValue(type, out var dict)) return Task.FromResult(0);

            return Task.FromResult(dict.TryRemove(entity.Id, out var _) ? 1 : 0);
        }

        public Task<T> FindByIdAsync<T>(int id) where T : IEntity
        {
            var type = typeof(T);
            if (_store.TryGetValue(type, out var dict) && dict.TryGetValue(id, out var entity))
            {
                return Task.FromResult((T)entity);
            }

            return Task.FromResult(default(T)!);
        }

        public IQueryable<T> Query<T>() where T : IEntity
        {
            var type = typeof(T);
            var dict = _store.GetOrAdd(type, _ => new ConcurrentDictionary<int, IEntity>());
            return dict.Values.Cast<T>().AsQueryable();
        }
    }
}
