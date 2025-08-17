using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Managers;
using Template.Infrastructure.Config;
using Template.Shared.Extensions;

namespace Template.Infrastructure.Helper
{
    public class JwtHelper
    {
        private readonly IConfigManager _configManager;

        public JwtHelper(IConfigManager configManager)
        {
            _configManager = configManager;
        }
        public JwtConfig GetJwtOptions()
        {

            return new JwtConfig()
            {
                Audience = _configManager.GetConfig("JwtOptions:Audience"),
                Issuer = _configManager.GetConfig("JwtOptions:Issuer"),
                TokenExpiration = _configManager.GetConfig("JwtOptions:TokenExpiration").ToInt(),
                SecurityKey = _configManager.GetConfig("JwtOptions:SecurityKey"),
                RefreshTokenExpiration = _configManager.GetConfig("JwtOptions:RefreshTokenExpiration").ToInt()
            };
        }

        public JwtConfig GetJwtOptions(int tokenExpiration)
        {
            return new JwtConfig()
            {
                Audience = _configManager.GetConfig("JwtOptions:Audience"),
                Issuer = _configManager.GetConfig("JwtOptions:Issuer"),
                TokenExpiration = tokenExpiration,
                SecurityKey = _configManager.GetConfig("JwtOptions:SecurityKey"),
                RefreshTokenExpiration = _configManager.GetConfig("JwtOptions:RefreshTokenExpiration").ToInt()
            };
        }
    }
}
