using Auth.Application.Commands;
using Auth.Application.MediatR;
using Auth.Application.QueryAndHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Constants;

namespace KO.WebAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : KOControllerBase
    {
        public UserController(IMediatKO mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand user)
        {

            user.Role = Role.User;
            return await ExecuteSignAsync<CreateUserCommand, CreateUserHandler>(user);
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAddmin([FromBody] CreateUserCommand user)
        {
            user.Role = Role.Admin;
            return await ExecuteSignAsync<CreateUserCommand, CreateUserHandler>(user);
        }
        [HttpPost("Super-admin")]
        public async Task<IActionResult> CreateSuperAddmin([FromBody] CreateUserCommand user)
        {
            user.Role = Role.SuperAdmin;
            return await ExecuteSignAsync<CreateUserCommand, CreateUserHandler>(user);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand login)
        {
            return await ExecuteSignAsync<LoginCommand, LoginHandler>(login);
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePassword)
        {
            return await ExecuteAsync<ChangePasswordCommand, ChangePasswordCommandHandler>(changePassword);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand forgetPassword)
        {
            return await ExecuteAsync<ForgetPasswordCommand, ForgetPasswordHandler>(forgetPassword);
        }
        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordCommand recoverPassword)
        {
            return await ExecuteAsync<RecoverPasswordCommand, RecoveryPasswordHandler>(recoverPassword);
        }

        [HttpPost("set-new-password")]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordCommand setNewPassword)
        {
            return await ExecuteAsync<SetNewPasswordCommand, SetNewPasswordHandler>(setNewPassword);
        }

        [HttpDelete("hard-delete")]
        public async Task<IActionResult> HardDelete([FromBody] HardDeleteCommand hardDelete)
        {
            //hardDelete.HardDelete+=
            return await ExecuteAsync<HardDeleteCommand, HardDeleteHandler>(hardDelete);
        }
        [HttpDelete("soft-delete")]
        public async Task<IActionResult> SoftDelete([FromBody] SoftDeleteCommand softDelete)
        {
            return await ExecuteAsync<SoftDeleteCommand, SoftDeleteHandler>(softDelete);
        }

        [HttpPost("undo-soft-delete")]
        public async Task<IActionResult> UndoSoftDelete([FromBody] UndoSoftDeleteCommand undoSoftDelete)
        {
            return await ExecuteAsync<UndoSoftDeleteCommand, UndoSoftDeleteHandler>(undoSoftDelete);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return BadRequest("Refresh token has expired or has never be set.");
            return await RefreshTokenAsync<RefreshTokenCommand, RefreshTokenHandler>(new RefreshTokenCommand() { RefreshToken = refreshToken });
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            return await QueryAsync<AllUserQuery, AllUserHandler>(new AllUserQuery());
        }

        [HttpGet("soft-deleted-user")]
        public async Task<IActionResult> GetSoftDeleteUser()
        {
            return await QueryAsync<SoftDeleteUserQuery, SoftDeletedUserHandler>(new SoftDeleteUserQuery());
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return await QueryAsync<UserById, UserByIdQueryHadler>(new UserById() { Id = id });
        }

    }
}
