using Auth.Application.RepositoryContracts;
using Auth.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using static Auth.Application.Dependency.AppServiceDependency;

namespace Auth.Persistence
{
    /// <summary>
    /// Provides extension methods for registering data-related services to the dependency injection container.
    /// </summary>
    public static class DataDependency
    {
        /// <summary>
        /// Registers data-related services to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to register the services to.</param>
        /// <param name="secretKey">The secret key to use for authentication.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection DataServiceCollection(this IServiceCollection services,string secretKey)
        {
            services.AppServiceCollection(secretKey);
            services.AddScoped<IUserRepo, UserRepo>();
            return services;
        }
    }
}
