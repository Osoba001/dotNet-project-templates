using Auth.Application.MediatR;
using Auth.Application.Models;
using Auth.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.QueryAndHandlers
{
    /// <summary>
    /// Represents a query to retrieve a user by their ID.
    /// </summary>
    public class UserById:IQuery
    {
        /// <summary>
        /// Gets or sets the ID of the user to retrieve.
        /// </summary>
        public required Guid Id { get; set; }
    }

    /// <summary>
    /// Represents a handler for the UserById query.
    /// </summary>
    public class UserByIdQueryHadler : IQueryHandler<UserById>
    {
        /// <summary>
        /// Handles the UserById query by retrieving the user from the service and returning it in a KOActionResult.
        /// </summary>
        /// <param name="query">The UserById query to handle.</param>
        /// <param name="service">The service wrapper to use to retrieve the user.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A KOActionResult containing the user if found, or an error message if not found.</returns>
        public async Task<KOActionResult> HandlerAsync(UserById query, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var result = new KOActionResult();
            var resp=await service.UserRepo.FindById(query.Id);
            if (resp == null)
            {
                result.AddError("User Not found");
                return result;
            }
            UserResponse user = resp;
            result.data = user;
            return result;
        }
    }
}
