using Auth.Application.Models;
using Auth.Application.RepositoryContracts;
using Auth.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Utilities.Responses;

namespace Auth.Persistence.Repositories
{
    internal class BaseRepo<T> : IBaseRepo<T> where T : ModelBase
    {
        private readonly AuthDbContext _context;

        public BaseRepo(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<KOActionResult> Add(T entity)
        {
            _context.Add(entity);
            return await _context.SaveActionAsync();
        }

        public async Task<KOActionResult<T>> AddAndReturn(T entity)
        {
            _context.Add(entity);
            return await _context.SaveActionAsync(entity);
        }

        public async Task<KOActionResult> Delete(T entity)
        {
            _context.Remove(entity);
            return await _context.SaveActionAsync();

        }

        public async Task<KOActionResult<T>> DeleteAndReturn(T entity)
        {
            _context.Remove(entity);
            return await _context.SaveActionAsync(entity);
        }

        public virtual async Task<T?> FindOneByPredicate(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }
        public virtual async Task<List<T>> FindByPredicate(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }


        public async Task<KOActionResult> Update(T entity)
        {
            _context.Update(entity);
            return await _context.SaveActionAsync();
        }

        public async Task<KOActionResult<T>> UpdateAndReturn(T entity)
        {
            _context.Update(entity);
            return await _context.SaveActionAsync(entity);
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> FindById(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<KOActionResult> DeleteRange(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return await _context.SaveActionAsync();
        }

        public async Task<KOActionResult> DeleteRange(List<Guid> ids)
        {
            var allEntities = _context.Set<T>().IgnoreQueryFilters();
            var entities = allEntities.IntersectBy(ids, x => x.Id);
            _context.Set<T>().RemoveRange(entities);
            return await _context.SaveActionAsync();
        }
    }
}
