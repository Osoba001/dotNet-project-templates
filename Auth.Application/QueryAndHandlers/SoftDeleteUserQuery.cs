using Auth.Application.Helper;
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
    public class SoftDeleteUserQuery:IQuery
    {
        
    }

    public class SoftDeletedUserHandler : IQueryHandler<SoftDeleteUserQuery>
    {
        public async Task<KOActionResult> HandlerAsync(SoftDeleteUserQuery query, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var response = new KOActionResult();
            response.data= (await service.UserRepo.IgnorQueryFilter(x => x.IsFalseDeleted)).ConvertUserList();
            return response;
        }
    }
}
