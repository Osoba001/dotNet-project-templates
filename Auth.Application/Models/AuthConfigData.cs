using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Models
{
    public class AuthConfigData
    {
        public required string SecretKey { get; set; }
        public required int AccessTokenLifespanInMins { get; set; }
        public required int RefreshTokenLifespanInMins { get; set; }
        public required int RecoveryPinLifeSpanInMins { get; set; }
    }
}
