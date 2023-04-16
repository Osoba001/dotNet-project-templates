using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Constants
{
    public static class ResponseMessage
    {
        public const string InvalidEmailOrPassword = "Email or password is incorrect.";
        public const string RecordNotFound = "Record is not found.";
        public const string UserNotFound = "User is not found.";
        public const string EmailAlreadyExist = "Email is already in used.";
        public const string InvalidPassword = "Password is not correct.";
        public const string SessionExpired = "Session has expired.";
        public const string IncorrectPin = "Incorrect pin";
        public const string InvalidToken = "Invalid Token";
    }
}
