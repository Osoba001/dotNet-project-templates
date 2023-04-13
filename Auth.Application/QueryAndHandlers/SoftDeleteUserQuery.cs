using Auth.Application.CommandAndHandlers.Helper;
using Auth.Application.MediatR;
using Auth.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.QueryAndHandlers
{
    /// <summary>
    /// Represents a query for getting all soft deleted users.
    /// </summary>
    public class SoftDeleteUserQuery:IQuery
    {
        
    }

    /// <summary>
    /// Represents a query handler for the <see cref="SoftDeleteUserQuery"/> query.
    /// </summary>
    public class SoftDeletedUserHandler : IQueryHandler<SoftDeleteUserQuery>
    {
        /// <summary>
        /// Handles the <see cref="SoftDeleteUserQuery"/> query asynchronously and returns a <see cref="KOActionResult"/> containing the list of all soft deleted users.
        /// </summary>
        /// <param name="query">The <see cref="SoftDeleteUserQuery"/> query.</param>
        /// <param name="service">The <see cref="IServiceWrapper"/> service wrapper.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to cancel the asynchronous operation (optional).</param>
        /// <returns>A <see cref="KOActionResult"/> containing the list of all soft deleted users.</returns>
        public async Task<KOActionResult> HandlerAsync(SoftDeleteUserQuery query, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            return new KOActionResult
            {
                data = (await service.UserRepo.IgnorQueryFilter(x => x.IsFalseDeleted)).ConvertUserList()
            };
        }
    }
}
