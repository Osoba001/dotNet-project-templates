using Auth.Application.Models;
using System.Linq.Expressions;
using Utilities.Responses;

namespace Auth.Application.RepositoryContracts
{
    public interface IUserRepo:IBaseRepo<UserModel>
    {
        Task<List<UserModel>> IgnorQueryFilter(Expression<Func<UserModel, bool>> predicate);
       
    }
}
