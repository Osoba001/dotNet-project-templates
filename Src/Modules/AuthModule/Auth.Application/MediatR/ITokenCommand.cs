namespace Auth.Application.MediatR
{
    /// <summary>
    /// Represents a command that generates a new refresh token.
    /// </summary>
    /// <remarks>
    /// This interface defines a command that can also generates token. Any class that implements this
    /// interface must provide its own implementation of the <see cref="ICommand"/> interface and the
    /// <see cref="GeneratedRefreshToken"/> event.
    /// </remarks>
    public interface ITokenCommand : ICommand
    {
        /// <summary>
        /// Raised when a new refresh token has been generated.
        /// </summary>
        /// <remarks>
        /// This event is raised when a new refresh token has been generated. Any subscribers to this event
        /// can receive the Refresh token as a <see cref="string"/> parameter  and manage it.
        /// </remarks>
        event EventHandler<string>? GeneratedRefreshToken;
    }
}
