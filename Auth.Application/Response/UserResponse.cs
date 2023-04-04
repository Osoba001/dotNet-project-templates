using Auth.Application.Models;
using Auth.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Response
{
    public class UserResponse
    {
        public required Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }

        public static implicit operator UserResponse(UserModel model)
        {
            return new UserResponse() { Email = model.Email, Id = model.Id, UserName = model.UserName, Role = model.Role.ToString() };
        }
    }
}
