using Auth.Application.AuthServices;
using Auth.Application.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.MediatR
{
    /// <summary>
    /// A wrapper for all the services handlers needs
    /// </summary>
    public interface IServiceWrapper
    {
        /// <summary>
        /// User repository contract.
        /// </summary>
        IUserRepo UserRepo { get;}

        /// <summary>
        /// Authentication service contract
        /// </summary>
        IAuthService AuthService { get;}
    }
}
