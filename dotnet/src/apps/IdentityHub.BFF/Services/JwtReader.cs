using System.IdentityModel.Tokens.Jwt;

namespace IdentityHub.BFF.Services
{
    public sealed class JwtReader
    {
        private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new ();

        public JwtReaderDTO Extract(string token)
        {
            var result =  _jwtSecurityTokenHandler.ReadJwtToken(token);

            var userId = result.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)!.Value;
            var login = result.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)!.Value;
            var expire = DateTimeOffset.FromUnixTimeSeconds(long.Parse(result.Claims.First(x => x.Type == "exp").Value)).UtcDateTime;

            return new JwtReaderDTO(userId, login, expire);
        }
    }

    public sealed record JwtReaderDTO(string UserId, string Login, DateTime ExpiredTime);
}