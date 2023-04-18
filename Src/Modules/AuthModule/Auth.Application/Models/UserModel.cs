using Utilities.Constants;

namespace Auth.Application.Models
{
    /// <summary>
    /// The model class representing a user.
    /// </summary>
    public class UserModel : ModelBase
    {
        /// <summary>
        /// The username of the user.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// The user type.
        /// </summary>
        public required Role Role { get; set; }

        /// <summary>
        /// The email of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The password hash of the user.
        /// </summary>
        public byte[]? PasswordHash { get; set; }

        /// <summary>
        /// The password salt of the user.
        /// </summary>
        public byte[]? PasswordSalt { get; set; }

        /// <summary>
        /// The refresh token of the user.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// A flag indicating whether the user has been falsely deleted.
        /// </summary>
        public bool IsFalseDeleted { get; set; }

        /// <summary>
        /// The date when the user was falsely deleted.
        /// </summary>
        public DateTime FalseDeletedDate { get; set; }

        /// <summary>
        /// The time when the user is allowed to set a new password.
        /// </summary>
        public DateTime AllowSetNewPassword { get; set; }

        /// <summary>
        /// The time when the refresh token of the user will expire.
        /// </summary>
        public DateTime RefreshTokenExpireTime { get; set; }

        /// <summary>
        /// The password recovery pin of the user.
        /// </summary>
        public int PasswordRecoveryPin { get; set; }

        /// <summary>
        /// The time when the password recovery pin was created.
        /// </summary>
        public DateTime RecoveryPinCreationTime { get; set; }
    }

}
