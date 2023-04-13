using Utilities.Responses;

namespace Auth.Application.MediatR
{
    /// <summary>
    /// Interface for Mediator that executes Commands and Queries.
    /// </summary>
    public interface IMediatKO
    {
        /// <summary>
        /// Executes the given Command using the specified CommandHandler.
        /// </summary>
        /// <typeparam name="TCommand">The type of the Command to execute.</typeparam>
        /// <typeparam name="TCommandHandler">The type of the CommandHandler to use for executing the Command.</typeparam>
        /// <param name="command">The Command to execute.</param>
        /// <returns>A Task containing the result of the Command execution.</returns>
        Task<KOActionResult> ExecuteCommandAsync<TCommand,TCommandHandler>(TCommand command)
           where TCommand : ICommand
           where TCommandHandler : ICommandHandler<TCommand>;

        /// <summary>
        /// Executes the given Query using the specified QueryHandler.
        /// </summary>
        /// <typeparam name="TQuery">The type of the Query to execute.</typeparam>
        /// <typeparam name="TQueryHandler">The type of the QueryHandler to use for executing the Query.</typeparam>
        /// <param name="query">The Query to execute.</param>
        /// <returns>A Task containing the result of the Query execution.</returns>
        Task<KOActionResult> QueryAsync<TQuery,TQueryHandler>(TQuery query)
            where TQuery : IQuery
            where TQueryHandler : IQueryHandler<TQuery>;
    }
}
