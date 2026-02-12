using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Contracts.Response.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityHub.AuthService.Application.Services
{
    public sealed class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
    {
        public string GenerateRefreshToken() => Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

        public string GenerateAccessToken(UserResponse response)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, response.Id.ToString()),
                new (JwtRegisteredClaimNames.Name, response.Login),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiris = DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:AccessTokenExpirationMinutes"]));
            var token = new JwtSecurityToken
                (
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims: claims,
                    expires: expiris,
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
