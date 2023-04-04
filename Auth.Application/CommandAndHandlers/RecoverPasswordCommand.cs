using Auth.Application.MediatR;
using System.Text.RegularExpressions;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    public class RecoverPasswordCommand : ICommand
    {
        public required string Email { get; set; }
        public required int RecoveryPin { get; set; }
        public KOActionResult Validate()
        {
            var result = new KOActionResult();
            if (!Regex.IsMatch(Email, emailPattern))
                result.AddError("Invalid email address.");

            return result;
        }
        static string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    }

    public class RecoveryPasswordHandler : ICommandHandler<RecoverPasswordCommand>
    {
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
