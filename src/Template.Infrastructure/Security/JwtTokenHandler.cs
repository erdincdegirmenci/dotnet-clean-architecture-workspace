using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Template.Infrastructure.Config;

namespace Template.Infrastructure.Security
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        public string GenerateAccessToken(string userId, List<string> roles, JwtConfig config)
        {
            try
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userId)
            };
                if (roles != null)
                {
                    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: config.Issuer,
                    audience: config.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(config.TokenExpiration),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GenerateAccessToken(string userId, List<string> roles, List<string> permissions, JwtConfig config)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userId)
        };
            if (roles != null) claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            if (permissions != null) claims.AddRange(permissions.Select(p => new Claim("permission", p)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config.Issuer,
                audience: config.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(config.TokenExpiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public JwtSecurityToken ResolveAccessToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        public string GetUserId(string token)
        {
            var jwt = ResolveAccessToken(token);
            return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
        }

        public List<string> GetUserRoles(string token)
        {
            var jwt = ResolveAccessToken(token);
            return jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }
    }
}
