using Auth.Application.EventData;
using Auth.Application.Models;
using Auth.Application.RepositoryContracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Application.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;
        private readonly AuthConfigData _configData;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userRepo">The user repository.</param>
        /// <param name="optionsConfigData">The authentication configuration data.</param>
        public AuthService(IUserRepo userRepo, IOptions<AuthConfigData> optionsConfigData)
        {
            _userRepo = userRepo;
            _configData = optionsConfigData.Value;
        }

        /// <summary>
        /// Hashes the provided password and sets the password hash and salt for the provided user.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <param name="user">The user whose password hash and salt will be set.</param>
        public void PasswordManager(string password, UserModel user)
        {
            using var hmc = new HMACSHA512();
            byte[] passwordSalt = hmc.Key;
            byte[] passwordHash = hmc.ComputeHash(Encoding.ASCII.GetBytes(password));
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }

        /// <summary>
        /// Creates a new JWT access token and a refresh token for the provided user.
        /// </summary>
        /// <param name="user">The user for whom to generate the access and refresh tokens.</param>
        /// <returns>A new <see cref="TokenModelArgs"/> containing the JWT access token and refresh token.</returns>
        public async Task<TokenModelArgs> TokenManager(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedKey = Encoding.ASCII.GetBytes(_configData.SecretKey);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                            new Claim(ClaimTypes.Email,user.Email),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                            new Claim(ClaimTypes.Role,user.Role.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddMinutes(_configData.AccessTokenLifespanInMins),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.RefreshToken = $"{RandomToken()}{user.Email}";
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddMinutes(_configData.RefreshTokenLifespanInMins);

            await _userRepo.Update(user);
            return new TokenModelArgs()
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = user.RefreshToken,
            };
        }
        private static string RandomToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        /// <summary>
        /// Verifies whether the provided password matches the password hash and salt for the provided user.
        /// </summary>
        /// <param name="password">The password to be verified.</param>
        /// <param name="user">The user whose password hash and salt will be used for verification.</param>
        /// <returns>True if the provided password matches the password hash and salt for the provided user, false otherwise.</returns>
        public bool VerifyPassword(string password, UserModel user)
        {
            using var hmc = new HMACSHA512(user.PasswordSalt!);
            byte[] computed = hmc.ComputeHash(Encoding.ASCII.GetBytes(password));
            return computed.SequenceEqual(user.PasswordHash!);
        }
    }
}
