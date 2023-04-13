using Auth.Application.MediatR;
using Auth.Application.Responses;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    /// <summary>
    /// Represents a command for undoing a soft-delete operation on a user.
    /// </summary>
    public class UndoSoftDeleteCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the ID of the user to undo the soft-delete for.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// Validates the UndoSoftDeleteCommand.
        /// </summary>
        /// <returns>The validation result as a KOActionResult.</returns>
        public KOActionResult Validate()
        {
            return new KOActionResult();
        }

        /// <summary>
        /// Occurs when an undo-soft-delete operation is executed on a user.
        /// </summary>
        public event EventHandler<UserArgs>? UndoSoftDelete;

        /// <summary>
        /// Raises the UndoSoftDelete event.
        /// </summary>
        /// <param name="userArgs">The user arguments associated with the undo-soft-delete operation.</param>

        internal virtual void OnUndoSoftDelete(UserArgs userArgs)
        {
            UndoSoftDelete?.Invoke(this, userArgs);
        }
    }

    /// <summary>
    /// Represents a command handler for undoing a soft-delete operation on a user.
    /// </summary>
    public class UndoSoftDeleteHandler : ICommandHandler<UndoSoftDeleteCommand>
    {
        public async Task<KOActionResult> HandleAsync(UndoSoftDeleteCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            /// <summary>
            /// Executes the undo-soft-delete operation.
            /// </summary>
            /// <param name="command">The UndoSoftDeleteCommand to execute.</param>
            /// <param name="service">The IServiceWrapper instance.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>A KOActionResult instance.</returns>
            var result = new KOActionResult();
            var user = await service.UserRepo.FindById(command.Id);
            if (user is null)
            {
                result.AddError("User not found.");
                return result;
            }
            user.IsFalseDeleted = false;
            user.FalseDeletedDate = DateTime.UtcNow;
            var res = await service.UserRepo.Update(user);
            if (!res.IsSuccess)
            {
                result.AddError(res.ReasonPhrase);
                return result;
            }
            UserArgs resp = user!;
            command.OnUndoSoftDelete(resp);
            return result;
        }
    }
}
