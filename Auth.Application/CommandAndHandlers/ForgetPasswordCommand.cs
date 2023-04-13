using Auth.Application.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.Responses;
using Utilities.Validations;

namespace Auth.Application.Commands
{
    /// <summary>
    /// Represents a command to initiate the password recovery process for a user.
    /// </summary>
    /// <remarks>
    /// This command is used to initiate the password recovery process for a user by providing their email address. The command performs validation on the email address to ensure that it is a valid email address before proceeding with the password recovery process.
    /// </remarks>
    public class ForgetPasswordCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the email address of the user to recover the password for.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Validates the email address specified in the command.
        /// </summary>
        /// <returns>An instance of KOActionResult indicating the result of the validation.</returns>
        
        public KOActionResult Validate()
        {
            var result = new KOActionResult();
            if (!Email.IsEmailValid())
                result.AddError("Invalid email address.");

            return result;
        }
    }


    /// <summary>
    /// Represents a handler for the ForgetPasswordCommand, which initiates the password recovery process for a user.
    /// </summary>
    /// <remarks>
    /// This handler takes a ForgetPasswordCommand instance as input and attempts to initiate the password recovery process for the user specified in the command. If the user exists in the system, a random PIN is generated and assigned to the user's account, which is used to verify the user's identity during the password recovery process.
    /// </remarks>
    public class ForgetPasswordHandler : ICommandHandler<ForgetPasswordCommand>
    {
        /// <summary>
        /// Handles the specified ForgetPasswordCommand instance and initiates the password recovery process for the user specified in the command.
        /// </summary>
        /// <param name="command">The ForgetPasswordCommand instance specifying the email address of the user to recover the password for.</param>
        /// <param name="service">The service wrapper instance providing access to the required services and repositories.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation if required.</param>
        /// <returns>An instance of KOActionResult indicating the result of the operation, including the PIN generated for the user if successful.</returns>
        public async Task<KOActionResult> HandleAsync(ForgetPasswordCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user = await service.UserRepo.FindOneByPredicate(x => x.Email == command.Email.ToLower().Trim());

            if (user is null)
                result.AddError("User not found.");
            else
            {
                int pin = RandomPin();
                user.PasswordRecoveryPin = pin;
                user.RecoveryPinCreatedTime = DateTime.UtcNow;
                var rs = await service.UserRepo.Update(user);
                if (!rs.IsSuccess)
                    result.AddError(rs.ReasonPhrase);
                else
                    result.data = pin;
            }
            return result;
        }

        /// <summary>
        /// Generates a random 5-digit PIN number to be used during the password recovery process.
        /// </summary>
        /// <returns>A random 5-digit PIN number.</returns>
        /// <remarks>
        /// This method generates a random 5-digit PIN number to be used during the password recovery process. The PIN number is used to verify the user's identity before allowing them to reset their password.
        /// </remarks>
        private static int RandomPin()
        {
            var random = new Random();
            return random.Next(10000, 99999);
        }
    }


}
