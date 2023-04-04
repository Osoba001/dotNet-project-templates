using Microsoft.Extensions.DependencyInjection;
using Utilities.Responses;

namespace Auth.Application.MediatR
{
    public class MediatKO : IMediatKO
    {
        private readonly IServiceProvider _serviceProvider;

        public MediatKO(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IServiceWrapper service => _serviceProvider.GetRequiredService<IServiceWrapper>();

        public Task<KOActionResult> ExecuteCommandAsync<TCommandHandler, TCommand>(TCommand command)
            where TCommandHandler : ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            var handler = (TCommandHandler)Activator.CreateInstance(typeof(TCommandHandler))!;
            return handler.HandleAsync(command, service);
        }

        public Task<KOActionResult> QueryAsync<TQueryHandler, TQuery>(TQuery query)
            where TQueryHandler : IQueryHandler<TQuery>
            where TQuery : IQuery
        {
            var handler = (TQueryHandler)Activator.CreateInstance(typeof(TQueryHandler))!;
            return handler.HandlerAsync(query, service);
        }
    }
}
