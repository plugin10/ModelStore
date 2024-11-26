using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ModelStore.Application
{
    public class TokenGenerator
    {
        public string GenerateToken(int id, string name, string email, string userType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // on production store this key securely
            var key = "MyVerySecretTokenGeneratorKeyOnlyUseInDevelopEnviroment"u8.ToArray();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, email),
                new(JwtRegisteredClaimNames.Email, email),
                new(ClaimTypes.Role, userType),
                new("id", id.ToString()),
                new("name", name),
                new("userType", userType)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = "https://user.localhost",
                Audience = "https://localhost",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}