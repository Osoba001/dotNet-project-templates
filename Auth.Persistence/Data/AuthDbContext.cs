using Auth.Application.Models;
using Microsoft.EntityFrameworkCore;
using Utilities.Responses;

namespace Auth.Persistence.Data
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options) { }

        public DbSet<UserModel> UserTb { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntityConfig().Configure(modelBuilder.Entity<UserModel>());
        }

        public async Task<KOActionResult> SaveActionAsync()
        {
            var res = new KOActionResult();
            try
            {
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                res.AddError(ex.Message);
            }
            return res;
        }
        public async Task<KOActionResult<T>> SaveActionAsync<T>(T entity) where T : class
        {
            var res = new KOActionResult<T>();
            try
            {
                await SaveChangesAsync();
                res.Item = entity;
            }
            catch (Exception ex)
            {
                res.AddError(ex.Message);
            }
            return res;
        }
    }
}
