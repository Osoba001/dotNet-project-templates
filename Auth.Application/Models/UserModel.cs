using Auth.Application.Constanst;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Models
{
    public class UserModel:ModelBase
    {
        public required string UserName { get; set; }
        public required Role Role { get; set; }
        public required string Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsFalseDeleted { get; set; }
        public DateTime FalseDeletedDate { get; set; }
        public DateTime AllowSetNewPassword { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public int PasswordRecoveryPin { get; set; }
        public DateTime RecoveryPinCreatedTime { get; set; }
    }
}
