using Utilities.Responses;

namespace Auth.Application.MediatR
{
    /// <summary>
    /// This is a generic contract that every QueryHandler must implement before it can be called by QueryAsync of the custome mediator (MediatKO).
    /// It takes in the Query type.
    /// </summary>
    /// <typeparam name="TQuery">Query Type</typeparam>
    public interface IQueryHandler<TQuery> where TQuery : IQuery
    {
        /// <summary>
        /// A method that implement the logic of the query.
        /// </summary>
        /// <param name="query"> query parameter</param>
        /// <param name="service">A wrapper that contains all the service needed</param>
        /// <param name="cancellationToken">A token to cancle the execution</param>
        /// <returns> Task of KOACtionResult</returns>
        Task<KOActionResult> HandlerAsync(TQuery query, IServiceWrapper service, CancellationToken cancellationToken = default);
    }
}
