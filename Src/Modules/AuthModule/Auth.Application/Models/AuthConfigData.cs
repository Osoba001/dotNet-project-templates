using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Models
{
    /// <summary>
    /// Represents the configuration data needed for authentication.
    /// </summary>
    public class AuthConfigData
    {
        /// <summary>
        /// Gets or sets the secret key used to sign the access and refresh tokens.
        /// </summary>
        public required string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the lifespan of an access token in minutes.
        /// </summary>
        public required int AccessTokenLifespanInMins { get; set; }

        /// <summary>
        /// Gets or sets the lifespan of a refresh token in minutes.
        /// </summary>
        public required int RefreshTokenLifespanInMins { get; set; }
    }

}
