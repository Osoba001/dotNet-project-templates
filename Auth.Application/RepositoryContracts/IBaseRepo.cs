using Auth.Application.Models;
using System.Linq.Expressions;
using Utilities.Responses;

namespace Auth.Application.RepositoryContracts
{
    /// <summary>
    /// Represents the interface for a base repository.
    /// </summary>
    /// <typeparam name="T">The type of model the repository works with.</typeparam>
    public interface IBaseRepo<T> where T : ModelBase
    {
        /// <summary>
        /// Adds a new entity to the repository and returns a <see cref="KOActionResult{T}"/> instance containing the added entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A <see cref="KOActionResult{T}"/> instance containing the added entity.</returns>
        Task<KOActionResult<T>> AddAndReturn(T entity);

        /// <summary>
        /// Updates an existing entity in the repository and returns a <see cref="KOActionResult{T}"/> instance containing the updated entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A <see cref="KOActionResult{T}"/> instance containing the updated entity.</returns>
        Task<KOActionResult<T>> UpdateAndReturn(T entity);

        /// <summary>
        /// Deletes an existing entity from the repository and returns a <see cref="KOActionResult{T}"/> instance containing the deleted entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A <see cref="KOActionResult{T}"/> instance containing the deleted entity.</returns>
        Task<KOActionResult<T>> DeleteAndReturn(T entity);

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A <see cref="KOActionResult"/> instance.</returns>
        Task<KOActionResult> Add(T entity);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A <see cref="KOActionResult"/> instance.</returns>
        Task<KOActionResult> Update(T entity);

        /// <summary>
        /// Deletes an existing entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A <see cref="KOActionResult"/> instance.</returns>
        Task<KOActionResult> Delete(T entity);

        /// <summary>
        /// Deletes a range of entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>A <see cref="KOActionResult"/> instance.</returns>
        Task<KOActionResult> DeleteRange(List<T> entities);

        /// <summary>
        /// Gets all entities from the repository.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of entities.</returns>
        Task<List<T>> GetAll();

        /// <summary>
        /// Finds entities that match the specified predicate in the repository.
        /// </summary>
        /// <param name="predicate">The predicate used to filter the entities.</param>
        /// <returns>A <see cref="List{T}"/> of entities that match the predicate.</returns>
        Task<List<T>> FindByPredicate(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Finds the first entity that matches the specified predicate in the repository.
        /// </summary>
        /// <param name="predicate">The predicate used to filter the entities.</param>
        /// <returns>The first entity that matches the predicate, or <see langword="null"/> if no entity
        Task<T?> FindOneByPredicate(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Finds the entity with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the entity to find.</param>
        /// <returns>The entity with the specified ID, or <c>null</c> if no entity is found.</returns>
        Task<T?> FindById(Guid id);

        /// <summary>
        /// Deletes the entities with the specified IDs.
        /// </summary>
        /// <param name="ids">The IDs of the entities to delete.</param>
        /// <returns>An action result indicating whether the operation was successful.</returns>
        Task<KOActionResult> DeleteRange(List<Guid> ids);
    }
}

