using Auth.Application.AuthServices;
using Auth.Application.MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.Application.Dependency
{
    /// <summary>
    /// Provides service registration for JWT authentication in ASP.NET Core applications.
    /// </summary>
    public static class AppServiceDependency
    {
        /// <summary>
        /// Registers the required services for JWT authentication.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        /// <param name="secretKey">The secret key used for JWT authentication.</param>
        /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AppServiceCollection(this IServiceCollection services,string secretKey)
        {
            services.AddScoped<IMediatKO,MediatKO>();
            services.AddScoped<IServiceWrapper, ServiceWrapper>();
            services.AddScoped<IAuthSetup, AuthSetup>();
            services.JWTService(secretKey);
            services.AddAuthorization();
            return services;
        }

        /// <summary>
        /// Configures JWT authentication services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance to which the services will be added.</param>
        /// <param name="secretKey">The secret key used for JWT authentication.</param>
        /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
        private static IServiceCollection JWTService(this IServiceCollection services, string secretKey)
        {
            var key = Encoding.ASCII.GetBytes(secretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }
    }
    
}
