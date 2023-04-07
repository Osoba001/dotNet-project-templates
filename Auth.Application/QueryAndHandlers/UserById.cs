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
    public class UserById:IQuery
    {
        public required Guid Id { get; set; }
    }

    public class UserByIdQueryHadler : IQueryHandler<UserById>
    {
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
