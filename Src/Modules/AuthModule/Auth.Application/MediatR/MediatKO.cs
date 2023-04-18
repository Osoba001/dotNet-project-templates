using Microsoft.Extensions.DependencyInjection;
using Utilities.Responses;

namespace Auth.Application.MediatR
{

    internal class MediatKO : IMediatKO
    {
        private readonly IServiceProvider _serviceProvider;

        public MediatKO(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IServiceWrapper service => _serviceProvider.GetRequiredService<IServiceWrapper>();

        /// <summary>
        /// Executes the given Command using the specified CommandHandler.
        /// </summary>
        /// <typeparam name="TCommand">The type of the Command to execute.</typeparam>
        /// <typeparam name="TCommandHandler">The type of the CommandHandler to use for executing the Command.</typeparam>
        /// <param name="command">The Command to execute.</param>
        /// <returns>A Task containing the result of the Command execution.</returns>
        public Task<KOActionResult> ExecuteCommandAsync<TCommand,TCommandHandler>(TCommand command)
            where TCommandHandler : ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            var handler = Activator.CreateInstance<TCommandHandler>();
            return handler.HandleAsync(command, service);
        }

        public Task<KOActionResult> ExecuteTokenCommandAsync<TCommand, TCommandHandler>(TCommand command)
            where TCommand : ITokenCommand
            where TCommandHandler : ICommandHandler<TCommand>
        {
            var handler=Activator.CreateInstance<TCommandHandler>();
            return handler.HandleAsync(command, service);
        }

        /// <summary>
        /// Executes the given Query using the specified QueryHandler.
        /// </summary>
        /// <typeparam name="TQuery">The type of the Query to execute.</typeparam>
        /// <typeparam name="TQueryHandler">The type of the QueryHandler to use for executing the Query.</typeparam>
        /// <param name="query">The Query to execute.</param>
        /// <returns>A Task containing the result of the Query execution.</returns>
        public Task<KOActionResult> QueryAsync<TQuery,TQueryHandler>(TQuery query)
            where TQueryHandler : IQueryHandler<TQuery>
            where TQuery : IQuery
        {
            var handler = (TQueryHandler)Activator.CreateInstance(typeof(TQueryHandler))!;
            return handler.HandlerAsync(query, service);
        }
    }
}
