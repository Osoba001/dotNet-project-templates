using Auth.Application.Models;
using Auth.Application.RepositoryContracts;
using Auth.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Persistence.Repositories
{
    // <summary>
    /// Repository class for managing User models.
    /// </summary>
    internal class UserRepo:BaseRepo<UserModel>,IUserRepo
    {
        private readonly AuthDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepo"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context to use.</param>
        public UserRepo(AuthDbContext context):base(context) 
        {
            _context = context;
        }

        /// <summary>
        /// Finds all User models that match the specified predicate, ignoring any query filters.
        /// </summary>
        /// <param name="predicate">The predicate to use to filter the results.</param>
        /// <returns>A list of User models that match the specified predicate.</returns>
        public async Task<List<UserModel>> IgnorQueryFilter(Expression<Func<UserModel, bool>> predicate)
        {
            return await _context.UserTb
                .IgnoreQueryFilters()
                .Where(predicate)
                .ToListAsync();
        }

        /// <summary>
        /// Finds a single User model with the specified ID, ignoring any query filters.
        /// </summary>
        /// <param name="id">The ID of the User model to find.</param>
        /// <returns>The User model with the specified ID, or null if no matching User model is found.</returns>
        public override async Task<UserModel?> FindById(Guid id)
        {
            return await _context.UserTb
                 .IgnoreQueryFilters()
                 .Where(x => x.Id == id)
                 .FirstOrDefaultAsync();
        }
    }
}
