using Auth.Application.Models;
using System.Linq.Expressions;
using Utilities.Responses;

namespace Auth.Application.RepositoryContracts
{
    /// <summary>
    /// Repository interface for accessing user data.
    /// </summary>
    public interface IUserRepo:IBaseRepo<UserModel>
    {
        /// <summary>
        /// Gets a list of users ignoring any query filters applied to the repository.
        /// </summary>
        /// <param name="predicate">The predicate to filter the results.</param>
        /// <returns>A list of users.</returns>
        Task<List<UserModel>> IgnorQueryFilter(Expression<Func<UserModel, bool>> predicate);
       
    }
}
