using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TokenGenerator
{
    public class TokenGenerator
    {
        public static string Create()
        {
            var secretKey = "THIS_IS_MY_SUPER_SECRET_KEY_12345";
            var key = Encoding.UTF8.GetBytes(secretKey);

            Random rnd = new Random();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
    {
                    new Claim("playerId", rnd.Next(1,100).ToString()),
                    new Claim("name", "John Doe"),
                    new Claim("role", "admin"),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}
