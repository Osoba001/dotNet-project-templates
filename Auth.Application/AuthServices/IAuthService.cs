using Auth.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.AuthServices
{
    public interface IAuthService
    {
        Task<TokenModel> TokenManager(UserModel user);

        void PasswordManager(string password, UserModel user);

        bool VerifyPassword(string password, UserModel user);
    }
}
