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
    /// Command to set new password for a user using recovery pin
    /// </summary>
    public class SetNewPasswordCommand : ICommand
    {
        /// <summary>
        /// The email address of the user
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The recovery pin received by the user
        /// </summary>
        public required int RecoveryPin { get; set; }

        /// <summary>
        /// The new password to be set
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Validates the command parameters
        /// </summary>
        /// <returns>A KOActionResult object containing any validation errors</returns>
        public KOActionResult Validate()
        {
            var result = new KOActionResult();
            if (!Email.IsEmailValid())
                result.AddError("Invalid email address.");

            return result;
        }
    }


    /// <summary>
    /// Handler for setting a new password for a user.
    /// </summary>
    public class SetNewPasswordHandler : ICommandHandler<SetNewPasswordCommand>
    {
        /// <summary>
        /// Handles the command to set a new password for a user.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="service">The service wrapper.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="KOActionResult"/> object with the result of the operation.</returns>
        public async Task<KOActionResult> HandleAsync(SetNewPasswordCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user = await service.UserRepo.FindOneByPredicate(x => x.Email == command.Email.ToLower().Trim());

            if (user is null || user.PasswordRecoveryPin!=command.RecoveryPin)
            {
                result.AddError("user not found.");
                return result;
            }
            if (user.AllowSetNewPassword< DateTime.UtcNow.AddMinutes(-10))
            {
                result.AddError("Session expired.");
                return result;
            }
            service.AuthService.PasswordManager(command.Password, user);
            user.AllowSetNewPassword=DateTime.UtcNow.AddMinutes(-20);
            return await service.UserRepo.Update(user);
        }
    }
}
