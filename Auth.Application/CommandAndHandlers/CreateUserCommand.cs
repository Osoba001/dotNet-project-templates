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
    public class CreateUserCommand : ICommand
    {
        /// <summary>
        /// The role of the new user.
        /// </summary>

        [JsonIgnore]
        public required Role Role { get; set; }

        /// <summary>
        /// The email address of the new user. This is a required field.
        /// </summary>
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// The user name of the new user. This is a required field.
        /// </summary>
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
        /// Occurs when the user is soft-deleted.
        /// </summary>
        public event EventHandler<UserArgs>? CreatedUser;

        /// <summary>
        /// Raises the Created user event.
        /// </summary>
        /// <param name="args">The user arguments.</param>
        internal virtual void OnCreated(UserArgs args)
        {
            CreatedUser?.Invoke(this, args);
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
        /// <remarks>
        /// This method handles the CreateUserCommand asynchronously by first validating the email address and 
        /// checking if it is already in use. If the email address is valid and not in use, a new UserModel object
        /// is created with the specified email address, user name, and role, and the password is hashed using the 
        /// service's PasswordManager method. The new user is then added to the user repository using the AddAndReturn 
        /// method, and a token is generated using the service's TokenManager method. Finally, the OnCreated method 
        /// of the CreateUserCommand is called with the newly created user as the argument, and an instance of 
        /// KOActionResult is returned containing the token as the data property.
        /// </remarks>
        public async Task<KOActionResult> HandleAsync(CreateUserCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var response=new KOActionResult();
            string email = command.Email.ToLower().Trim();
            var user=await service.UserRepo.FindOneByPredicate(x=>x.Email==email);
            if (user is not null)
            {
                response.AddError("Email is already in used.");
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
            command.OnCreated(result.Item!);
            response.data=service.AuthService.TokenManager(result.Item!);
            return response;

        }
    }
}
