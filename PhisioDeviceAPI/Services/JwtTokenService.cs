using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PhisioDeviceAPI.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(int userId, string email, string name, string role = "User");
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(int userId, string email, string name, string role = "User")
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key") ?? "QWET234TQ3456BTWERTHG[JP2IO13JR09213JU59034UTG9ewdfgewt234562316qw#r235rfqEAfg53u7y34567456i8u78p[--p-][890p-789o576u457u856896570789-=90[732NJIODFHNJUOHNBUbnxuibhq23ui9rh123";
            var issuer = jwtSection.GetValue<string>("Issuer") ?? "PhisioDeviceAPI";
            var audience = jwtSection.GetValue<string>("Audience") ?? "PhisioDeviceClient";
            var minutes = jwtSection.GetValue<int>("AccessTokenMinutes", 60);

            var claims = new List<Claim>
            {
                new Claim("userId", userId.ToString()),
                new Claim("email", email ?? string.Empty),
                new Claim("name", name ?? string.Empty),
                new Claim(ClaimTypes.Role, role ?? "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


