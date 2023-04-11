using Auth.Application.MediatR;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    /// <summary>
    /// Change existing password Command.
    /// </summary>
    public class ChangePasswordCommand : ICommand
    {
        public required Guid Id { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
        public KOActionResult Validate()
        {
            var result = new KOActionResult();
            return result;
        }
    }
    /// <summary>
    /// This class is the handler for ChangePasswordCommd. 
    /// </summary>
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
    {
      
       
        public async Task<KOActionResult> HandleAsync(ChangePasswordCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result=new KOActionResult();
            var user = await service.UserRepo.FindById(command.Id);
            if (user == null)
            {
                result.AddError("User not found.");
                return result;
            }
            if (!service.AuthService.VerifyPassword(command.OldPassword,user))
            {
                result.AddError("Old password is not correct.");
                return result;
            }
            service.AuthService.PasswordManager(command.NewPassword,user);
            return await service.UserRepo.Update(user);
        }
    }
}
