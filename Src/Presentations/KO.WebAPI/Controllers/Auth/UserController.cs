using Auth.Application.Commands;
using Auth.Application.EventData;
using Auth.Application.MediatR;
using Auth.Application.QueryAndHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace KO.WebAPI.Controllers.Auth
{
    /// <summary>
    /// Represents a controller for handling user-related API requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AuthControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserController(IMediatKO mediator) : base(mediator) { }

        /// <summary>
        /// Creates a new user with the given information.
        /// The command has an event that returns the created user.
        /// </summary>
        /// <param name="user">The command object containing user information.</param>
        /// <returns>An <see cref="IActionResult"/> object representing the HTTP response to the request.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand user)
        {
            //user.CreatedUser += User_CreatedUser;
            user.Role = Role.User;
            return await ExecuteTokenAsync<CreateUserCommand, CreateUserHandler>(user);
        }

        /// <summary>
        /// Creates a new admin user with the given information.
        /// The command has an event that returns the created user.
        /// </summary>
        /// <param name="user">The command object containing user information.</param>
        /// <returns>An <see cref="IActionResult"/> object representing the HTTP response to the request.</returns>
        [HttpPost("admin")]
        public async Task<IActionResult> CreateAddmin([FromBody] CreateUserCommand user)
        {
            user.Role = Role.Admin;
            return await ExecuteTokenAsync<CreateUserCommand, CreateUserHandler>(user);
        }

        /// <summary>
        /// Creates a new super admin user with the given information.
        /// The command has an event that returns the created user.
        /// </summary>
        /// <param name="user">The command object containing user information.</param>
        /// <returns>An <see cref="IActionResult"/> object representing the HTTP response to the request.</returns>
        [HttpPost("Super-admin")]
        public async Task<IActionResult> CreateSuperAddmin([FromBody] CreateUserCommand user)
        {
            user.Role = Role.SuperAdmin;
            return await ExecuteTokenAsync<CreateUserCommand, CreateUserHandler>(user);
        }

        /// <summary>
        /// Authenticates a user with the given login information.
        /// </summary>
        /// <param name="login">The command object containing login information.</param>
        /// <returns>An <see cref="IActionResult"/> object representing the HTTP response to the request.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand login)
        {
            return await ExecuteTokenAsync<LoginCommand, LoginHandler>(login);
        }

        /// <summary>
        /// Changes the password of a user.
        /// </summary>
        /// <param name="changePassword">The command object containing the user ID and new password.</param>
        /// <returns>An <see cref="IActionResult"/> object representing the HTTP response to the request.</returns>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePassword)
        {
            return await ExecuteAsync<ChangePasswordCommand, ChangePasswordCommandHandler>(changePassword);
        }

        /// <summary>
        /// Sends an email to a user to reset their password.
        /// The command has an event that returns a password recovery pin.
        /// </summary>
        /// <param name="forgetPassword">The command object containing the user's email.</param>
        /// <returns>An <see cref="IActionResult"/> object representing the HTTP response to the request.</returns>
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand forgetPassword)
        {
            return await ExecuteAsync<ForgetPasswordCommand, ForgetPasswordHandler>(forgetPassword);
        }

        /// <summary>
        /// Recover the user's password by sending a recovery email.
        /// </summary>
        /// <param name="recoverPassword">The command for recovering a password.</param>
        /// <returns>The result of executing the recovery password command.</returns>
        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoveryPasswordCommand recoverPassword)
        {
            return await ExecuteAsync<RecoveryPasswordCommand, RecoveryPasswordHandler>(recoverPassword);
        }

        /// <summary>
        /// Set a new password for the user.
        /// </summary>
        /// <param name="setNewPassword">The command for setting a new password.</param>
        /// <returns>The result of executing the set new password command.</returns>
        [HttpPost("set-new-password")]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordCommand setNewPassword)
        {
            return await ExecuteAsync<SetNewPasswordCommand, SetNewPasswordHandler>(setNewPassword);
        }

        

        /// <summary>
        /// Soft delete the user's account and mark as deleted in the database.
        /// </summary>
        /// <param name="softDelete">The command for soft deleting a user's account.</param>
        /// <returns>The result of executing the soft delete command.</returns>
        [HttpDelete("soft-delete")]
        public async Task<IActionResult> SoftDelete([FromBody] SoftDeleteCommand softDelete)
        {
            return await ExecuteAsync<SoftDeleteCommand, SoftDeleteHandler>(softDelete);
        }

        /// <summary>
        /// Undo a soft delete on the user's account and mark as active in the database.
        /// </summary>
        /// <param name="undoSoftDelete">The command for undoing a soft delete on a user's account.</param>
        /// <returns>The result of executing the undo soft delete command.</returns>
        [HttpPost("undo-soft-delete")]
        public async Task<IActionResult> UndoSoftDelete([FromBody] UndoSoftDeleteCommand undoSoftDelete)
        {
            return await ExecuteAsync<UndoSoftDeleteCommand, UndoSoftDeleteHandler>(undoSoftDelete);
        }

        /// <summary>
        /// Hard delete the user's account and all associated data.
        /// </summary>
        /// <param name="hardDelete">The command for hard deleting a user's account.</param>
        /// <returns>The result of executing the hard delete command.</returns>
        [HttpDelete("hard-delete")]
        public async Task<IActionResult> HardDelete([FromBody] HardDeleteCommand hardDelete)
        {
            //hardDelete.HardDelete+=
            return await ExecuteAsync<HardDeleteCommand, HardDeleteHandler>(hardDelete);
        }
        /// <summary>
        /// Refresh the user's authentication token using their refresh token.
        /// </summary>
        /// <returns>The result of executing the refresh token command.</returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return BadRequest("Refresh token has expired or has never be set.");
            return await ExecuteTokenAsync<RefreshTokenCommand, RefreshTokenHandler>(new RefreshTokenCommand() { RefreshToken = refreshToken });
        }

        /// <summary>
        /// Get all users in the database.
        /// </summary>
        /// <returns>The result of executing the all user query.</returns>
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            return await QueryAsync<AllUserQuery, AllUserHandler>(new AllUserQuery());
        }

        /// <summary>
        /// Get all soft deleted users in the database.
        /// </summary>
        /// <returns>The result of executing the soft deleted user query.</returns>
        [HttpGet("soft-deleted-user")]
        public async Task<IActionResult> GetSoftDeleteUser()
        {
            return await QueryAsync<SoftDeleteUserQuery, SoftDeletedUserHandler>(new SoftDeleteUserQuery());
        }

        /// <summary>
        /// Get a specific user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The result of executing the user by ID query.</returns>
        [HttpGet("/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return await QueryAsync<UserById, UserByIdQueryHadler>(new UserById() { Id = id });
        }

    }
}
