using Auth.Application.RepositoryContracts;
using Auth.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using static Auth.Application.Dependency.AppServiceDependency;

namespace Auth.Persistence
{
    public static class DataDependency
    {
        public static IServiceCollection DataServiceCollection(this IServiceCollection services,string secretKey)
        {
            services.AppServiceCollection(secretKey);
            services.AddScoped<IUserRepo, UserRepo>();
            return services;
        }
    }
}
