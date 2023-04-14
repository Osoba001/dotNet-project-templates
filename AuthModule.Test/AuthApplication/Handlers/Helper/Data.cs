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

namespace AuthModule.Test.AuthApplication.Handlers.Helper
{
    internal static class Data
    {
        
        public static void PasswordManager(string password, UserModel user)
        {
            //AuthService server = new(null,null);
            //server.PasswordManager(password, user);
            using var hmc = new HMACSHA512();
            byte[] passwordSalt = hmc.Key;
            byte[] passwordHash = hmc.ComputeHash(Encoding.ASCII.GetBytes(password));
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }
        public static List<UserModel> Users => CreateUsers();
        public static TokenModelArgs tokenModel = new TokenModelArgs() { AccessToken = Guid.NewGuid().ToString(), RefreshToken = Guid.NewGuid().ToString() };
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
                PasswordManager("password" + i, user);
                //AuthService server = new(null,null);
                //server.PasswordManager(password, user);
                users.Add(user);
            }
            return users;
        }
    }
}
