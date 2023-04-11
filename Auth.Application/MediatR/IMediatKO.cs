using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.MediatR
{
    /// <summary>
    /// This contract has the generic methods ExecuteCommandAsync and QueryAsync. 
    /// It act as mediator b/w Presentation (Auth Module Controllers) and and application commands and queries.
    /// </summary>
    public interface IMediatKO
    {
        /// <summary>
        /// This generic method of type Command and commandHandler is used to execute commands.
        /// </summary>
        /// <typeparam name="TCommand">Command type</typeparam>
        /// <typeparam name="TCommandHandler"> CommandHandler Type</typeparam>
        /// <param name="command">command parameter</param>
        /// <returns>Task of KOActionResult</returns>
        Task<KOActionResult> ExecuteCommandAsync<TCommand,TCommandHandler>(TCommand command)
           where TCommand : ICommand
           where TCommandHandler : ICommandHandler<TCommand>;

        /// <summary>
        /// This generic method of type Query and QueryHandler is used to execute queries.
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TQueryHandler">queryHandler type</typeparam>
        /// <param name="query">query</param>
        /// <returns>Task of KOActionResult</returns>
        Task<KOActionResult> QueryAsync<TQuery,TQueryHandler>(TQuery query)
            where TQuery : IQuery
            where TQueryHandler : IQueryHandler<TQuery>;
    }
}
