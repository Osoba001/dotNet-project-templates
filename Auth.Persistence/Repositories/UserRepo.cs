using Auth.Application.Models;
using Auth.Application.RepositoryContracts;
using Auth.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Persistence.Repositories
{
    internal class UserRepo:BaseRepo<UserModel>,IUserRepo
    {
        private readonly AuthDbContext _context;

        public UserRepo(AuthDbContext context):base(context) 
        {
            _context = context;
        }
        public async Task<List<UserModel>> IgnorQueryFilter(Expression<Func<UserModel, bool>> predicate)
        {
            return await _context.UserTb
                .IgnoreQueryFilters()
                .Where(predicate)
                .ToListAsync();
        }

        public override async Task<UserModel?> FindById(Guid id)
        {
            return await _context.UserTb
                 .IgnoreQueryFilters()
                 .Where(x => x.Id == id)
                 .FirstOrDefaultAsync();
        }
    }
}
