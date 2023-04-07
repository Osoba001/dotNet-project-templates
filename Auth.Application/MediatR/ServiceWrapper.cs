using Auth.Application.AuthServices;
using Auth.Application.RepositoryContracts;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application.MediatR
{
    internal class ServiceWrapper : IServiceWrapper
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceWrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        private IUserRepo? userRepo;

        public IUserRepo UserRepo
        {
            get 
            {
                userRepo ??= _serviceProvider.GetRequiredService<IUserRepo>();
                return userRepo;
            }

        }


        private IAuthService? authService;

        public IAuthService AuthService
        {
            get 
            {
                authService ??= _serviceProvider.GetRequiredService<IAuthService>();
                return authService;
            }
        }

    }
}
