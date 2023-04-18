using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.EventData
{
    /// <summary>
    /// Represents a model for a JWT token and its refresh token.
    /// </summary>
    public class TokenModel:EventArgs
    {
        /// <summary>
        /// Gets or sets the access token value.
        /// </summary>
        public required string AccessToken { get; set; }
        /// <summary>
        /// Gets or sets the refresh token value.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
