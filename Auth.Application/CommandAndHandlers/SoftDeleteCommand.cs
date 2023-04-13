using Auth.Application.MediatR;
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
    /// Command for soft deleting an entity.
    /// </summary>
    public class SoftDeleteCommand : ICommand
    {
        /// <summary>
        /// The ID of the entity to be soft deleted.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// Validates the command.
        /// </summary>
        /// <returns>An instance of <see cref="KOActionResult"/>.</returns>
        public KOActionResult Validate()
        {
            return new KOActionResult();
        }

        /// <summary>
        /// Event that is raised after the entity has been soft deleted.
        /// </summary>
        public event EventHandler<UserArgs>? SoftDeleted;

        /// <summary>
        /// Raises the <see cref="SoftDeleted"/> event with the specified <paramref name="args"/>.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        internal virtual void OnSoftDelete(UserArgs args)
        {
            SoftDeleted?.Invoke(this, args);
        }
    }
    /// <summary>
    /// Command handler for SoftDeleteCommand.
    /// </summary>
    public class SoftDeleteHandler : ICommandHandler<SoftDeleteCommand>
    {

        /// <summary>
        /// Handles the SoftDeleteCommand by marking the corresponding user as not deleted.
        /// </summary>
        /// <param name="command">The SoftDeleteCommand to be handled.</param>
        /// <param name="service">The service wrapper instance.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A KOActionResult indicating whether the operation was successful or not.</returns>
        public async Task<KOActionResult> HandleAsync(SoftDeleteCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user = await service.UserRepo.FindById(command.Id);
            if (user is null)
            {
                result.AddError("User not found.");
                return result;
            }
            user.IsFalseDeleted = true;
            user.FalseDeletedDate = DateTime.UtcNow;
            var res = await service.UserRepo.Update(user);
            if (!res.IsSuccess)
            {
                result.AddError(res.ReasonPhrase);
                return result;
            }
            UserArgs resp = user!;
            command.OnSoftDelete(resp);
            return result;
        }
    }
}
