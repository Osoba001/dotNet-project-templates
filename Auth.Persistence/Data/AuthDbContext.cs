using Auth.Application.Models;
using Microsoft.EntityFrameworkCore;
using Utilities.Responses;

namespace Auth.Persistence.Data
{

    /// <summary>
    /// Represents the authentication database context.
    /// </summary>
    public class AuthDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthDbContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="UserModel"/> entity set.
        /// </summary>
        public DbSet<UserModel> UserTb { get; set; }

        /// <summary>
        /// Configures the database schema for this context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntityConfig().Configure(modelBuilder.Entity<UserModel>());
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>A <see cref="KOActionResult"/> indicating the success or failure of the save operation.</returns>
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

        /// <summary>
        /// Saves all changes made in this context to the underlying database and returns the entity being saved.
        /// </summary>
        /// <typeparam name="T">The type of the entity being saved.</typeparam>
        /// <param name="entity">The entity to save.</param>
        /// <returns>A <see cref="KOActionResult{T}"/> indicating the success or failure of the save operation, and the saved entity.</returns>
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
