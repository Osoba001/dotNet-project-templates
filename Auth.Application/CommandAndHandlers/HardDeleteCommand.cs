using Auth.Application.MediatR;
using Auth.Application.Models;
using Auth.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    /// <summary>
    /// Represents a command for permanently deleting a user.
    /// </summary>
    public class HardDeleteCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the ID of the user to be deleted.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// Validates the command.
        /// </summary>
        /// <returns>A <see cref="KOActionResult"/> instance containing validation errors, if any.</returns>
        public KOActionResult Validate()
        {
            return new KOActionResult();
        }

        /// <summary>
        /// Event raised when a user is permanently deleted.
        /// </summary>
        public event EventHandler<UserArgs>? HardDelete;

        /// <summary>
        /// Raises the <see cref="HardDelete"/> event.
        /// </summary>
        /// <param name="user">The user that was deleted.</param>
        internal virtual void OnHardDelete(UserArgs user)
        {
            HardDelete?.Invoke(this, user);
        }
    }

    /// <summary>
    /// Handles the HardDeleteCommand by deleting the specified user from the database.
    /// </summary>
    public class HardDeleteHandler : ICommandHandler<HardDeleteCommand>
    {
        /// <summary>
        /// Handles the HardDeleteCommand by deleting the specified user from the database.
        /// </summary>
        /// <param name="command">The HardDeleteCommand to handle.</param>
        /// <param name="service">The service wrapper.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A KOActionResult indicating the result of the operation.</returns>
        public async Task<KOActionResult> HandleAsync(HardDeleteCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user = await service.UserRepo.FindById(command.Id);
            if (user is null)
            {
                result.AddError(UserNotFound);
                return result;
            }
            var res = await service.UserRepo.Delete(user);
            if (!res.IsSuccess)
            {
                result.AddError(res.ReasonPhrase);
                return result;
            }
            UserArgs resp = user!;
            command.OnHardDelete(resp);
            return result;
        }
    }
}
