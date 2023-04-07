using Auth.Application.Constanst;
using Auth.Application.MediatR;
using Auth.Application.Models;
using Auth.Application.RepositoryContracts;
using Auth.Application.Response;
using Auth.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    public class CreateUserCommand : ICommand
    {
        public readonly Role Role= Role.User;
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public KOActionResult Validate()
        {
            var result= new KOActionResult();
            if (!Regex.IsMatch(Email,emailPattern))
                result.AddError("Invalid email address.");
            
            return result;
        }
        static string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        public event EventHandler<UserArgs>? SoftDeleted;

        internal virtual void OnCreated(UserArgs args)
        {
            SoftDeleted?.Invoke(this, args);
        }
    }

    public class CreateUserHandler : ICommandHandler<CreateUserCommand>
    {
        public async Task<KOActionResult> HandleAsync(CreateUserCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var response=new KOActionResult();
            string email = command.Email.ToLower().Trim();
            var user=await service.UserRepo.FindOneByPredicate(x=>x.Email==email);
            if (user is not null)
            {
                response.AddError("Email is already in used.");
                return response;
            }
            UserModel userModel = new() { Email = email, Role = command.Role, UserName = command.UserName };
            service.AuthService.PasswordManager(command.Password,userModel);
            var result= await service.UserRepo.AddAndReturn(userModel);
            if(!result.IsSuccess)
            {
                response.AddError(result.ReasonPhrase);
                return response;
            }
            command.OnCreated(result.Item!);
            response.data=service.AuthService.TokenManager(result.Item!);
            return response;

        }
    }
}
