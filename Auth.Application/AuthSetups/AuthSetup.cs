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
    public class AuthSetup : IAuthSetup
    {
        private readonly IUserRepo _userRepo;
        private readonly AuthConfigData _configData;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthSetup"/> class.
        /// </summary>
        /// <param name="userRepo">The user repository.</param>
        /// <param name="optionsConfigData">The authentication configuration data.</param>
        public AuthSetup(IUserRepo userRepo, IOptions<AuthConfigData> optionsConfigData)
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
            byte[] passwordHash = hmc.ComputeHash(Encoding.UTF8.GetBytes(password));
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
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
            byte[] computed = hmc.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(user.PasswordHash!);
        }

        /// <summary>
        /// Creates a new JWT access token and a refresh token for the provided user.
        /// </summary>
        /// <param name="user">The user for whom to generate the access and refresh tokens.</param>
        /// <returns>A new <see cref="TokenModel"/> containing the JWT access token and refresh token.</returns>
        public async Task<TokenModel> TokenManager(UserModel user)
        {

            List<Claim> claims = CreateClaims(user);

            SecurityTokenDescriptor tokenDescriptor = CreateTokenDescriptor(claims);

            TokenModel tokens = CreateToken(user, tokenDescriptor);

            await _userRepo.Update(user);
            return tokens;
        }

        private SecurityTokenDescriptor CreateTokenDescriptor(List<Claim> claims)
        {
            var encodedKey = Encoding.UTF8.GetBytes(_configData.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddMinutes(_configData.AccessTokenLifespanInMins),
            };
            return tokenDescriptor;
        }

        private TokenModel CreateToken(UserModel user, SecurityTokenDescriptor tokenDescriptor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.RefreshToken = $"{RandomToken()}{user.Email}";
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddMinutes(_configData.RefreshTokenLifespanInMins);
            var tokens = new TokenModel()
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = user.RefreshToken,
            };
            
            return tokens;
        }

        private static List<Claim> CreateClaims(UserModel user)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                            new Claim(ClaimTypes.Email,user.Email),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                            new Claim(ClaimTypes.Role,user.Role.ToString()),
            };
        }

        private static string RandomToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

       
    }
}
