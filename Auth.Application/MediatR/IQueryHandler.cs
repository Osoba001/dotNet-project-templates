using Utilities.Responses;

namespace Auth.Application.MediatR
{
    public interface IQueryHandler<TQuery> where TQuery : IQuery
    {
        Task<KOActionResult> HandlerAsync(TQuery query, IServiceWrapper service, CancellationToken cancellationToken = default);
    }
}
