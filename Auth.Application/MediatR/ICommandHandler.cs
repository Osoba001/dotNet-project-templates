using Auth.Application.RepositoryContracts;
using Utilities.Responses;

namespace Auth.Application.MediatR
{
    /// <summary>
    /// This is a contract that every commandHandler must implement before it can be called by ExecutCommandAsync of the custome mediator (MediatKO).
    /// It takes in the command type.
    /// This contract has only one method which has the code implementation of the command and return Task of KOActionResult. 
    /// The handler method can only be called if the command is valid.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to be execute</typeparam>
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// This is where the command logic is being implemented.
        /// </summary>
        /// <param name="command">The commnad to execute</param>
        /// <param name="service">This contains all the services</param>
        /// <param name="cancellationToken">This used to stop the execution</param>
        /// <returns> Task of KOActionResult</returns>
        Task<KOActionResult> HandleAsync(TCommand command, IServiceWrapper service, CancellationToken cancellationToken = default);
    }
}
