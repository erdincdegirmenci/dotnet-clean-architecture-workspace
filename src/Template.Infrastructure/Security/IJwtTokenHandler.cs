using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Infrastructure.Config;

namespace Template.Infrastructure.Security
{
    public interface IJwtTokenHandler
    {
        string GenerateAccessToken(string userId, List<string> roles, JwtConfig config);
        string GenerateAccessToken(string userId, List<string> roles, List<string> permissions, JwtConfig config);
        string GenerateRefreshToken();
        JwtSecurityToken ResolveAccessToken(string token);
        string GetUserId(string token);
        List<string> GetUserRoles(string token);
    }
}
