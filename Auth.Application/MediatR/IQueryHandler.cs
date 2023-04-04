using Auth.Application.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.MediatR
{
    public interface IQueryHandler<TQuery> where TQuery : IQuery
    {
        Task<KOActionResult> HandlerAsync(TQuery query, IServiceWrapper service, CancellationToken cancellationToken = default);
    }
}
