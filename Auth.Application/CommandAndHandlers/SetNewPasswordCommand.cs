using Auth.Application.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    public class SetNewPasswordCommand : ICommand
    {
        public required string Email { get; set; }
        public required int RecoveryPin { get; set; }
        public required string Password { get; set; }
        public KOActionResult Validate()
        {
            var result = new KOActionResult();
            if (!Regex.IsMatch(Email, emailPattern))
                result.AddError("Invalid email address.");

            return result;
        }
        static string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    }

    public class SetNewPasswordHandler : ICommandHandler<SetNewPasswordCommand>
    {
        public async Task<KOActionResult> HandleAsync(SetNewPasswordCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user = await service.UserRepo.FindOneByPredicate(x => x.Email == command.Email.ToLower().Trim());

            if (user is null || user.PasswordRecoveryPin!=command.RecoveryPin)
            {
                result.AddError("user not found.");
                return result;
            }
            if (user.AllowSetNewPassword< DateTime.UtcNow.AddMinutes(-10))
            {
                result.AddError("Session expired.");
                return result;
            }
            service.AuthService.PasswordManager(command.Password, user);
            user.AllowSetNewPassword=DateTime.UtcNow.AddMinutes(-20);
            return await service.UserRepo.Update(user);
        }
    }
}
