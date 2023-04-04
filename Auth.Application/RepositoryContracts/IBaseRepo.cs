using Auth.Application.Models;
using System.Linq.Expressions;
using Utilities.Responses;

namespace Auth.Application.RepositoryContracts
{
    public interface IBaseRepo<T> where T : ModelBase
    {
        Task<KOActionResult<T>> AddAndReturn(T entity);
        Task<KOActionResult<T>> UpdateAndReturn(T entity);
        Task<KOActionResult<T>> DeleteAndReturn(T entity);
        Task<KOActionResult> Add(T entity);
        Task<KOActionResult> Update(T entity);
        Task<KOActionResult> Delete(T entity);
        Task<KOActionResult> DeleteRange(List<T> entities);
        Task<List<T>> GetAll();
        Task<List<T>> FindByPredicate(Expression<Func<T, bool>> predicate);
        Task<T?> FindOneByPredicate(Expression<Func<T, bool>> predicate);
        Task<T?> FindById(Guid id);
        Task<KOActionResult> DeleteRange(List<Guid> ids);
    }
}

