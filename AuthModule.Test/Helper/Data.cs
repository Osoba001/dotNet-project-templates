using Auth.Application.AuthServices;
using Auth.Application.EventData;
using Auth.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace AuthModule.Test.Helper
{
    internal static class Data
    {
        public static List<UserModel> Users => CreateUsers();
        public static TokenModel tokenModel = new TokenModel() { AccessToken = Guid.NewGuid().ToString(), RefreshToken = Guid.NewGuid().ToString() };
        private static List<UserModel> CreateUsers()
        {
            List<UserModel> users = new();
            for (int i = 0; i < 4; i++)
            {
                var user = new UserModel
                {
                    Email = Guid.NewGuid().ToString(),
                    Id = Guid.NewGuid(),
                    UserName = Guid.NewGuid().ToString(),
                    Role = Role.User,
                };
                users.Add(user);
            }
            return users;
        }
    }
}
