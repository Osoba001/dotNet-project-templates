using Auth.Application.MediatR;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    /// <summary>
    /// Represents a command for changing a user's password.
    /// </summary>
    public class ChangePasswordCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the ID of the user whose password is being changed.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user's old password.
        /// </summary>
        public required string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the user's new password.
        /// </summary>
        public required string NewPassword { get; set; }

        /// <summary>
        /// Validates the command parameters.
        /// </summary>
        /// <returns>A <see cref="KOActionResult"/> object that contains the validation result.</returns>
        public KOActionResult Validate()
        {
            return new KOActionResult();
        }
    }

    /// <summary>
    /// Handler for the ChangePasswordCommand.
    /// </summary>
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
    {
        /// <summary>
        /// Handles the ChangePasswordCommand asynchronously.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="service">The service wrapper containing the necessary services.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A KOActionResult representing the outcome of the operation.</returns>
        public async Task<KOActionResult> HandleAsync(ChangePasswordCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();

            // Find the user with the specified ID
            var user = await service.UserRepo.FindById(command.Id);

            // If the user is not found, add an error to the result and return it
            if (user == null)
            {
                result.AddError("User not found.");
                return result;
            }

            // Verify the old password
            if (!service.AuthService.VerifyPassword(command.OldPassword, user))
            {
                result.AddError("Old password is not correct.");
                return result;
            }

            // Set the new password
            service.AuthService.PasswordManager(command.NewPassword, user);

            // Update the user and return the result of the update operation
            return await service.UserRepo.Update(user);
        }
    }
}