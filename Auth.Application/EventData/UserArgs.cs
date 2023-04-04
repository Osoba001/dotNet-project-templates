using Auth.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Responses
{
    public class UserArgs:EventArgs
    {          
        public required Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }

        public static implicit operator UserArgs(UserModel model)
        {
            return new UserArgs() { Email = model.Email, Id = model.Id, UserName = model.UserName, Role = model.Role.ToString() };
        }
    }
}
