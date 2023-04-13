using Auth.Application.Models;
using Utilities.Constants;

namespace Auth.Application.Responses
{
    /// <summary>
    /// Provides event arguments for user-related events.
    /// </summary>
    public class UserArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user's username.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's role.
        /// </summary>
        public required Role Role { get; set; }

        /// <summary>
        /// Implicitly converts a <see cref="UserModel"/> object to a <see cref="UserArgs"/> object.
        /// </summary>
        /// <param name="model">The <see cref="UserModel"/> object to convert.</param>
        /// <returns>A <see cref="UserArgs"/> object.</returns>

        public static implicit operator UserArgs(UserModel model)
        {
            return new UserArgs() { Email = model.Email, Id = model.Id, UserName = model.UserName, Role = model.Role };
        }
    }
}
