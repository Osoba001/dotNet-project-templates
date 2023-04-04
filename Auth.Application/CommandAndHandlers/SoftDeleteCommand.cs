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
    public class SoftDeleteCommand : ICommand
    {
        public required Guid Id { get; set; }
        public KOActionResult Validate()
        {
            return new KOActionResult();
        }
        public event EventHandler<UserArgs>? SoftDeleted;

        internal virtual void OnSoftDelete(UserArgs args)
        {
            SoftDeleted?.Invoke(this, args);
        }
    }

    public class SoftDeleteHandler : ICommandHandler<SoftDeleteCommand>
    {
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
