using Auth.Application.Models;
using Auth.Application.RepositoryContracts;
using Auth.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Utilities.Responses;

namespace Auth.Persistence.Repositories
{
    /// <summary>
    /// A base repository class that implements common database operations for entities of type T 
    /// in a database using Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of entity to be handled by this repository.</typeparam>
    internal class BaseRepo<T> : IBaseRepo<T> where T : ModelBase
    {
        private readonly AuthDbContext _context;
        /// <summary>
        /// Initializes a new instance of the BaseRepo class with the specified context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public BaseRepo(AuthDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds the specified entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>A KOActionResult indicating success or failure of the operation.</returns>
        public async Task<KOActionResult> Add(T entity)
        {
            _context.Add(entity);
            return await _context.SaveActionAsync();
        }

        /// <summary>
        /// Adds a new entity to the database and returns it.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A KOActionResult containing the added entity and indicating whether the operation was successful.</returns>
        public async Task<KOActionResult<T>> AddAndReturn(T entity)
        {
            _context.Add(entity);
            return await _context.SaveActionAsync(entity);
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to be deleted from the database.</param>
        /// <returns>A KOActionResult indicating success or failure of the operation.</returns>
        public async Task<KOActionResult> Delete(T entity)
        {
            _context.Remove(entity);
            return await _context.SaveActionAsync();

        }

        /// <summary>
        /// Deletes an entity from the database and returns it.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A KOActionResult containing the deleted entity and indicating whether the operation was successful.</returns>
        public async Task<KOActionResult<T>> DeleteAndReturn(T entity)
        {
            _context.Remove(entity);
            return await _context.SaveActionAsync(entity);
        }

        /// <summary>
        /// Finds an entity in the database based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate to use for the search.</param>
        /// <returns>The first entity that matches the predicate, or null if no entity is found.</returns>
        public virtual async Task<T?> FindOneByPredicate(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds entities in the database based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate to use for the search.</param>
        /// <returns>A list of entities that match the predicate.</returns>
        public virtual async Task<List<T>> FindByPredicate(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Updates the specified entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A KOActionResult indicating success or failure of the operation.</returns>
        public async Task<KOActionResult> Update(T entity)
        {
            _context.Update(entity);
            return await _context.SaveActionAsync();
        }
        /// <summary>
        /// Updates the specified entity in the database and returns the updated entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A KOActionResult containing the updated entity and indicating success or failure of the operation.</returns>
        public async Task<KOActionResult<T>> UpdateAndReturn(T entity)
        {
            _context.Update(entity);
            return await _context.SaveActionAsync(entity);
        }
        /// <summary>
        /// Retrieves all entities of the specified type from the database.
        /// </summary>
        /// <returns>A list of all entities of the specified type in the database.</returns>
        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        /// <summary>
        /// Retrieves the entity with the specified ID from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity with the specified ID, or null if no such entity exists.</returns>
        public virtual async Task<T?> FindById(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        /// <summary>
        /// Deletes the specified entities from the database.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>A KOActionResult indicating success or failure of the operation.</returns>
        public async Task<KOActionResult> DeleteRange(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return await _context.SaveActionAsync();
        }
        /// <summary>
        /// Deletes the entities with the specified IDs from the database.
        /// </summary>
        /// <param name="ids">The IDs of the entities to delete. </param>
        /// <returns>A KOActionResult indicating success or failure of the operation.</returns>
        public async Task<KOActionResult> DeleteRange(List<Guid> ids)
        {
            var allEntities = _context.Set<T>().IgnoreQueryFilters();
            var entities = allEntities.IntersectBy(ids, x => x.Id);
            _context.Set<T>().RemoveRange(entities);
            return await _context.SaveActionAsync();
        }
    }


  

}