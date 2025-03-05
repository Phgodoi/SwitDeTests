using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using minimal_api.API.Domain.Entities;
using minimal_api.API.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace minimal_api.API.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _key;

        public AuthService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"] ?? string.Empty;
        }
        public string GerarToken(Administrador adm)
        {
            if (string.IsNullOrEmpty(_key)) return string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim("Email", adm.Email),
            new Claim("Profile", adm.Profile),
            new Claim(ClaimTypes.Role, adm.Profile),
        };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
