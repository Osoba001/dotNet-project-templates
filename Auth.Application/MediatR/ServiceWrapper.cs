using Auth.Application.AuthServices;
using Auth.Application.RepositoryContracts;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application.MediatR
{
    /// <summary>
    /// Provides access to instances of various services through the application's service provider.
    /// </summary>
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceWrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets an instance of the user repository.
        /// </summary>
        public IUserRepo UserRepo
        {
            get
            {
                userRepo ??= _serviceProvider.GetRequiredService<IUserRepo>();
                return userRepo;
            }
        }
        private IUserRepo? userRepo;

        /// <summary>
        /// Gets an instance of the authentication service.
        /// </summary>
        public IAuthSetup AuthService
        {
            get
            {
                authService ??= _serviceProvider.GetRequiredService<IAuthSetup>();
                return authService;
            }
        }
        private IAuthSetup? authService;
    }
}
