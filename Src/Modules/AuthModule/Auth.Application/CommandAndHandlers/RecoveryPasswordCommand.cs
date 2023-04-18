using Auth.Application.MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Utilities.Responses;
using Utilities.Validations;

namespace Auth.Application.Commands
{
    /// <summary>
    /// Represents a command to recover a user's account.
    /// </summary>
    public class RecoveryPasswordCommand : ICommand
    {
        /// <summary>
        /// The email address of the user to recover the account for.
        /// </summary>
        [EmailAddress]
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
            return new KOActionResult();
        }
    }


    /// <summary>
    /// Handles the RecoverPasswordCommand to recover user's password.
    /// </summary>
    public class RecoveryPasswordHandler : ICommandHandler<RecoveryPasswordCommand>
    {

        /// <summary>
        /// Handles the recover password command.
        /// </summary>
        /// <param name="command">The recover password command.</param>
        /// <param name="service">The service wrapper.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the recover password operation.</returns>
        public async Task<KOActionResult> HandleAsync(RecoveryPasswordCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user = await service.UserRepo.FindOneByPredicate(x => x.Email == command.Email.ToLower().Trim());

            if (user is null)
            {
                result.AddError(UserNotFound);
                return result;
            }
            if (user.RecoveryPinCreationTime.AddMinutes(10) < DateTime.UtcNow)
            {
                result.AddError(SessionExpired);
                return result;
            }
            if(user.PasswordRecoveryPin!=command.RecoveryPin)
            {
                result.AddError(IncorrectPin);
                user.RecoveryPinCreationTime= DateTime.UtcNow.AddMinutes(-20);
                await service.UserRepo.Update(user);
                return result;
            }
            user.AllowSetNewPassword = DateTime.UtcNow;
            await service.UserRepo.Update(user);
            return result;

        }
    }
}
