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
    public class ForgetPasswordCommand : ICommand
    {
        public required string Email { get; set; }
        public KOActionResult Validate()
        {
            var result = new KOActionResult();
            if (!Regex.IsMatch(Email, emailPattern))
                result.AddError("Invalid email address.");

            return result;
        }
        static string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    }

    public class ForgetPasswordHandler : ICommandHandler<ForgetPasswordCommand>
    {
        public async Task<KOActionResult> HandleAsync(ForgetPasswordCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user = await service.UserRepo.FindOneByPredicate(x => x.Email == command.Email.ToLower().Trim());

            if (user is null)
                result.AddError("user not found.");
            else
            {
                int pin = RandomPin();
                user.PasswordRecoveryPin=pin;
                user.RecoveryPinCreatedTime=DateTime.UtcNow;
                var rs = await service.UserRepo.Update(user);
                if (!rs.IsSuccess)
                    result.AddError(rs.ReasonPhrase);
                else
                    result.data = pin;
            }
            return result;
        }
        private static int RandomPin()
        {
            var random = new Random();
            return random.Next(10000, 99999);
        }
    }

}
