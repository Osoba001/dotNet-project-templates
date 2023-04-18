using Auth.Application.Models;
using Auth.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.CommandAndHandlers.Helper
{
    /// <summary>
    /// Provides methods for converting between different types.
    /// </summary>
    public static class TypeConversion
    {
        /// <summary>
        /// Converts a list of <see cref="UserModel"/> objects to a list of <see cref="UserResponse"/> objects.
        /// </summary>
        /// <param name="list">The list of <see cref="UserModel"/> objects to convert.</param>
        /// <returns>A list of <see cref="UserResponse"/> objects.</returns>
        public static List<UserResponse> ConvertUserList(this List<UserModel> list)
        {
            var resp = new List<UserResponse>();
            foreach (var user in list)
            {
                resp.Add(user);
            }
            return resp;
        }
    }
}
