using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Config
{
    public class JwtConfig
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecurityKey { get; set; } = string.Empty;
        public int TokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
