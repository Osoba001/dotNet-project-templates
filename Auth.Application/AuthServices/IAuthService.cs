using Auth.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.AuthServices
{
    /// <summary>
    /// Interface for managing user authentication and token generation.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Generates an access and refresh token for the given user.
        /// </summary>
        /// <param name="user">The user for which to generate the tokens.</param>
        /// <returns>A <see cref="TokenModel"/> object containing the access and refresh tokens.</returns>
        Task<TokenModel> TokenManager(UserModel user);

        /// <summary>
        /// Generates and sets the password salt and hash for the given user.
        /// </summary>
        /// <param name="password">The password to hash and salt.</param>
        /// <param name="user">The user for which to generate the password hash and salt.</param>
        void PasswordManager(string password, UserModel user);

        /// <summary>
        /// Verifies whether the given password matches the user's stored password hash.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="user">The user to compare against.</param>
        /// <returns>True if the password matches the stored hash; otherwise, false.</returns>
        bool VerifyPassword(string password, UserModel user);
    }
}
