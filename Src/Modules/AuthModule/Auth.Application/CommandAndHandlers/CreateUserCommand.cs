using Auth.Application.EventData;
using Auth.Application.MediatR;
using Auth.Application.Models;
using Auth.Application.Responses;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Utilities.Constants;
using Utilities.Responses;
using Utilities.Validations;


namespace Auth.Application.Commands
{
    /// <summary>
    /// Represents a command to create a new user.
    /// </summary>
    public class CreateUserCommand : ITokenCommand
    {
        /// <summary>
        /// The role of the new user.
        /// </summary>

        [JsonIgnore]
        public  Role Role { get; set; }

        /// <summary>
        /// The email address of the new user. This is a required field.
        /// </summary>
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// The user name of the new user. This is a required field.
        /// </summary>
        [MaxLength(100)]
        public required string UserName { get; set; }

        /// <summary>
        /// The password of the new user. This is a required field.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Validates the command parameters.
        /// </summary>
        /// <returns>An instance of KOActionResult containing any validation errors.</returns>
        public KOActionResult Validate()
        {
            return new KOActionResult();
        }

        /// <summary>
        /// Occurs when the user is Created User and return the created user.
        /// </summary>
        public event EventHandler<UserArgs>? CreatedUser;
        /// <summary>
        /// Occurs when user has been created and return the user Refresh Token to the subscribers.
        /// </summary>
        public event EventHandler<string>? GeneratedRefreshToken;
        /// <summary>
        /// Raises the Created user event.
        /// </summary>
        /// <param name="UserArgs">The user arguments.</param>
        internal virtual void OnCreated(UserArgs UserArgs, string refrestoken)
        {
            CreatedUser?.Invoke(this, UserArgs);
            GeneratedRefreshToken?.Invoke(this, refrestoken);
        }
    }

    /// <summary>
    /// Handles the CreateUserCommand to create a new user.
    /// </summary>
    public class CreateUserHandler : ICommandHandler<CreateUserCommand>
    {
        /// <summary>
        /// Handles the CreateUserCommand asynchronously.
        /// </summary>
        /// <param name="command">The CreateUserCommand to handle.</param>
        /// <param name="service">The service wrapper containing the necessary services and repositories.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An instance of KOActionResult indicating the result of the command handling.</returns>
       
        public async Task<KOActionResult> HandleAsync(CreateUserCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var response=new KOActionResult();
            string email = command.Email.ToLower().Trim();
            var user=await service.UserRepo.FindOneByPredicate(x=>x.Email==email);
            if (user is not null)
            {
                response.AddError(EmailAlreadyExist);
                return response;
            }
            UserModel userModel = new() { Email = email, Role = command.Role, UserName = command.UserName };
            service.AuthService.PasswordManager(command.Password,userModel);
            var result= await service.UserRepo.AddAndReturn(userModel);
            if(!result.IsSuccess)
            {
                response.AddError(result.ReasonPhrase);
                return response;
            }
            var tokens = await service.AuthService.TokenManager(result.Item!);
            command.OnCreated(result.Item!, tokens.RefreshToken);
            response.Data= tokens.AccessToken;
            return response;
        }
    }
}
