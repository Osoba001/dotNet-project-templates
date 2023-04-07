using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.MediatR
{
    public interface IMediatKO
    {
        Task<KOActionResult> ExecuteCommandAsync<TCommand,TCommandHandler>(TCommand command)
           where TCommand : ICommand
           where TCommandHandler : ICommandHandler<TCommand>;

        Task<KOActionResult> QueryAsync<TQuery,TQueryHandler>(TQuery query)
            where TQuery : IQuery
            where TQueryHandler : IQueryHandler<TQuery>;
    }
}
