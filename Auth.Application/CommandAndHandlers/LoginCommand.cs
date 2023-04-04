using Auth.Application.MediatR;
using Auth.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    public class LoginCommand : ICommand
    {
        public required string Email { get; set; }
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

    public class LoginHandler : ICommandHandler<LoginCommand>
    {
        public async Task<KOActionResult> HandleAsync(LoginCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user= await service.UserRepo.FindOneByPredicate(x=>x.Email==command.Email.ToLower().Trim());

            if (user is null)
                result.AddError("Invalid login details.");
            else if(!service.AuthService.VerifyPassword(command.Password,user))
                result.AddError("Invalid login details.");
            else
                result.data = await service.AuthService.TokenManager(user);

            return result;
        }
    }
}
