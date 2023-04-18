using Auth.Application.EventData;
using Auth.Application.MediatR;
using Utilities.Responses;

namespace Auth.Application.Commands
{
    /// <summary>
    /// Represents a command to refresh an access token.
    /// </summary>
    public class RefreshTokenCommand : ITokenCommand
    {
        /// <summary>
        /// The refresh token associated with the access token to be refreshed.
        /// </summary>
        public required string RefreshToken { get; set; }

        /// <summary>
        /// Occurs when the user tokens have been Refreshed and return the user Refresh Token to the subscribers.
        /// </summary>
        public event EventHandler<string>? GeneratedRefreshToken;


        internal virtual void OnAuthenticated(string args)
        {
            GeneratedRefreshToken?.Invoke(this, args);
        }
        /// <summary>
        /// Validates the RefreshTokenCommand.
        /// </summary>
        /// <returns>A KOActionResult indicating if the command is valid or not.</returns>
        public KOActionResult Validate()
        {
            return new KOActionResult();
        }
    }

    /// <summary>
    /// Represents a handler for RefreshTokenCommand.
    /// </summary>
    public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand>
    {
        /// <summary>
        /// Handles a RefreshTokenCommand.
        /// </summary>
        /// <param name="command">The RefreshTokenCommand to be handled.</param>
        /// <param name="service">The service wrapper containing the necessary services.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
        /// <returns>A KOActionResult indicating the result of the operation.</returns>
        public async Task<KOActionResult> HandleAsync(RefreshTokenCommand command, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var user=await service.UserRepo.FindOneByPredicate(x=>x.RefreshToken==command.RefreshToken);
            if(user is null)
            {
                result.AddError(InvalidToken);
                return result;
            }
            if (user.RefreshTokenExpireTime < DateTime.UtcNow)
            {
                result.AddError(SessionExpired);
                return result;
            }
            var token = await service.AuthService.TokenManager(user);
            command.OnAuthenticated(token.RefreshToken);
            result.Data = token.AccessToken;
            return result;
        }
    }
}
