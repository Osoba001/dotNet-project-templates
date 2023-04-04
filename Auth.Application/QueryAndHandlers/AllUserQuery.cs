using Auth.Application.Helper;
using Auth.Application.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.QueryAndHandlers
{
    public class AllUserQuery:IQuery
    {
    }

    public class AllUserHandler : IQueryHandler<AllUserQuery>
    {
        public async Task<KOActionResult> HandlerAsync(AllUserQuery query, IServiceWrapper service, CancellationToken cancellationToken = default)
        {
            var response = new KOActionResult();
            response.data = (await service.UserRepo.GetAll()).ConvertUserList();
            return response;
        }
    }
}
