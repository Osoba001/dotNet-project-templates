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
            using var hmc = new HMACSHA512();
            byte[] passwordSalt = hmc.Key;
            byte[] passwordHash = hmc.ComputeHash(Encoding.ASCII.GetBytes(password));
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }
        public static List<UserModel> Users => CreateUsers();
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
                PasswordManager("password" + 1, user);
                users.Add(user);
            }
            return users;
        }
    }
}
