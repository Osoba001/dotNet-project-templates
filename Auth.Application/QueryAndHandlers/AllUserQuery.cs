using Auth.Application.CommandAndHandlers.Helper;
using Auth.Application.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.QueryAndHandlers
{
    /// <summary>
    /// Represents a query to retrieve all users.
    /// </summary>
    public class AllUserQuery:IQuery
    {
    }

    /// <summary>
    /// Represents a handler for the <see cref="AllUserQuery"/>.
    /// </summary>
    public class AllUserHandler : IQueryHandler<AllUserQuery>
    {
        /// <summary>
        /// Handles the specified <see cref="AllUserQuery"/> and returns a <see cref="KOActionResult"/> containing the list of all users.
        /// </summary>
        /// <param name="query">The query to handle.</param>
        /// <param name="service">The service wrapper.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="KOActionResult"/> containing the list of all users.</returns>
        public async Task<KOActionResult> HandlerAsync(AllUserQuery query, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            return new KOActionResult
            {
                data = (await service.UserRepo.GetAll()).ConvertUserList()
            };
        }
    }
}
