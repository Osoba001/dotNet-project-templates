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
    public class HardDeleteCommand : ICommand
    {
        public required Guid Id { get; set; }
        public KOActionResult Validate()
        {
            return new KOActionResult();
        }
        public event EventHandler<UserArgs>? HardDelete;
        internal virtual void OnHardDelete(UserArgs user)
        {
            HardDelete?.Invoke(this, user);
        }

    }

    public class HardDeleteHandler : ICommandHandler<HardDeleteCommand>
    {
        public async Task<KOActionResult> HandleAsync(HardDeleteCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user = await service.UserRepo.FindById(command.Id);
            if (user is null)
            {
                result.AddError("User not found.");
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
