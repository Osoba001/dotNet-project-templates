using Auth.Application.MediatR;
using Auth.Application.Responses;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    public class UndoSoftDeleteCommand : ICommand
    {
        public required Guid Id { get; set; }
        public KOActionResult Validate()
        {
            return new KOActionResult();
        }
        public event EventHandler<UserArgs>? UndoSoftDelete;

        internal virtual void OnUndoSoftDelete(UserArgs userArgs)
        {
            UndoSoftDelete?.Invoke(this, userArgs);
        }
    }

    public class UndoSoftDeleteHandler : ICommandHandler<UndoSoftDeleteCommand>
    {
        public async Task<KOActionResult> HandleAsync(UndoSoftDeleteCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
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
