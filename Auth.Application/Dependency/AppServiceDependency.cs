using Auth.Application.AuthServices;
using Auth.Application.MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.Application.Dependency
{
    public static class AppServiceDependency
    {
        public static IServiceCollection AppServiceCollection(this IServiceCollection services,string secretKey)
        {
            services.AddScoped<IServiceWrapper, ServiceWrapper>();
            services.AddScoped<IAuthService, AuthService>();
            services.JWTService(secretKey);
            services.AddAuthorization();
            return services;
        }

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
