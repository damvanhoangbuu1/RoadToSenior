using _1.CleanArchitecture.Domain.Common;
using _1.CleanArchitecture.Domain.Entities;
using _2.CleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _3.CleanArchitecture.Infrastructure.Services.Commons
{
    public class TokenService : ITokenService
    {
        private readonly Jwt _jwt;

        public TokenService(IOptions<Jwt> jwtOptions)
        {
            _jwt = jwtOptions.Value;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}