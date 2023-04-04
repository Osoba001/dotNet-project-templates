using Auth.Application.Models;
using Auth.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Helper
{
    public static class TypeConversion
    {
        public static List<UserResponse> ConvertUserList(this List<UserModel> list)
        { 
            var resp=new List<UserResponse>();
            foreach (var user in list)
            {
                resp.Add(user);
            }
            return resp;
        }
    }
}
