using GoodMoodPerfumeBot.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoodMoodPerfumeBot.Services
{
    public class AuthService
    {
        private readonly string secretKey;
        public AuthService(IConfiguration configuration)
        {
            this.secretKey = configuration.GetSection("AuthSettings:SecretKey").Value ?? string.Empty;
        }

        public string GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, user.UserRole)
                }),

                Expires = DateTime.UtcNow.AddHours(4)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
