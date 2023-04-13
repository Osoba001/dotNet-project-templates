using Auth.Application.MediatR;
using System.Text.RegularExpressions;
using Utilities.Responses;
using Utilities.Validations;

namespace Auth.Application.Commands
{
    /// <summary>
    /// Represents a command to recover a user's account.
    /// </summary>
    public class RecoverPasswordCommand : ICommand
    {
        /// <summary>
        /// The email address of the user to recover the account for.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The recovery PIN provided to the user to verify their identity.
        /// </summary>
        public required int RecoveryPin { get; set; }

        /// <summary>
        /// Validates the command's properties.
        /// </summary>
        /// <returns>A KOActionResult representing the result of the validation.</returns>
        public KOActionResult Validate()
        {
            var result = new KOActionResult();
            if (!Email.IsEmailValid())
                result.AddError("Invalid email address.");

            return result;
        }
    }


    /// <summary>
    /// Handles the RecoverPasswordCommand to recover user's password.
    /// </summary>
    public class RecoveryPasswordHandler : ICommandHandler<RecoverPasswordCommand>
    {

        /// <summary>
        /// Handles the recover password command.
        /// </summary>
        /// <param name="command">The recover password command.</param>
        /// <param name="service">The service wrapper.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the recover password operation.</returns>
        public async Task<KOActionResult> HandleAsync(RecoverPasswordCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user = await service.UserRepo.FindOneByPredicate(x => x.Email == command.Email.ToLower().Trim());

            if (user is null)
            {
                result.AddError("user not found.");
                return result;
            }
            if (user.RecoveryPinCreatedTime.AddMinutes(10) < DateTime.UtcNow)
            {
                result.AddError("Session expired.");
                return result;
            }
            if(user.PasswordRecoveryPin!=command.RecoveryPin)
            {
                result.AddError("Invalid Pin.");
                user.RecoveryPinCreatedTime= DateTime.UtcNow.AddMinutes(-20);
                await service.UserRepo.Update(user);
                return result;
            }
            user.AllowSetNewPassword = DateTime.UtcNow;
            await service.UserRepo.Update(user);
            return result;

        }
    }
}
