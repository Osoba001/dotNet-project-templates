using Auth.Application.AuthServices;
using Auth.Application.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.MediatR
{
    public interface IServiceWrapper
    {
        IUserRepo UserRepo { get;}

        IAuthService AuthService { get;}
    }
}
