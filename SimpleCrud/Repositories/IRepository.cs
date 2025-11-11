using SimpleCrud.Entities;

namespace SimpleCrud.Repositories
{
    public interface IRepository
    {
        /// <summary>
        /// Creates a new entity instance in the repository
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <param name="entity"></param>
        Task AddAsync<T>(T entity) where T : IEntity;

        /// <summary>
        /// Updates an existing entity instance in the repository
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <param name="entity">Entity instance with the updated values.</param>
        Task UpdateAsync<T>(T entity) where T : IEntity;

        /// <summary>
        /// Deleted an entity instance from the repository
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <param name="entity">The entity instance to be deleted</param>
        /// <returns>Number of rows of deleted entities related to the ID (returns 0 if nothing was found)</returns>
        Task<int> DeleteAsync<T>(T entity) where T : IEntity;

        /// <summary>
        /// Retrieves an entity by its ID
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <param name="id">The ID value of the entity</param>
        /// <returns>The entity found instance, returns null if no entity with the corresponding ID was found.</returns>
        Task<T> FindByIdAsync<T>(int id) where T : IEntity;

        /// <summary>
        /// Query entities of type T
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <returns>An IQueryable of the entity type</returns>
        IQueryable<T> Query<T>() where T : IEntity;
    }
}
