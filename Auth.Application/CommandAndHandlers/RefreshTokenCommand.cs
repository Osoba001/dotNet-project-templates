using Auth.Application.MediatR;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    public class RefreshTokenCommand : ICommand
    {
        public required string RefreshToken { get; set; }
        public KOActionResult Validate()
        {
            return new KOActionResult();
        }
    }

    public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand>
    {
        public async Task<KOActionResult> HandleAsync(RefreshTokenCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user=await service.UserRepo.FindOneByPredicate(x=>x.RefreshToken==command.RefreshToken);
            if(user is null)
            {
                result.AddError("Invalid Token");
                return result;
            }
            if (user.RefreshTokenExpireTime < DateTime.UtcNow)
            {
                result.AddError("Session expired. Try login again.");
                return result;
            }
            result.data = await service.AuthService.TokenManager(user);
            return result;
        }
    }
}
