using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities.Validations
{
    public static class CommandValidation
    {
        static string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        public static bool IsEmailValid(this string email)
        {
            if (Regex.IsMatch(email, emailPattern))
                return true;
            return false;
        }
    }
}
