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
    internal class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;
        private readonly AuthConfigData _configData;
        public AuthService(IUserRepo userRepo, IOptions<AuthConfigData> optionsConfigData)
        {
            _userRepo = userRepo;
            _configData = optionsConfigData.Value;
        }
        public void PasswordManager(string password, UserModel user)
        {
            using var hmc = new HMACSHA512();
            byte[] passwordSalt = hmc.Key;
            byte[] passwordHash = hmc.ComputeHash(Encoding.ASCII.GetBytes(password));
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }

        public async Task<TokenModel> TokenManager(UserModel user)
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
            return new TokenModel()
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = user.RefreshToken,
            };
        }
        private static string RandomToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public bool VerifyPassword(string password, UserModel user)
        {
            using var hmc = new HMACSHA512(user.PasswordSalt);
            byte[] computed = hmc.ComputeHash(Encoding.ASCII.GetBytes(password));
            return computed.SequenceEqual(user.PasswordHash);
        }
    }
}
