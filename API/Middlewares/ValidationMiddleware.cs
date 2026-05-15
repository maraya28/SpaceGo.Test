using Application;
using System.IdentityModel.Tokens.Jwt;

namespace API
{
    public class ValidationMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            var token = authHeader.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var playerId = jwtToken.Claims.FirstOrDefault(c => c.Type == Properties.PlayerId)?.Value;

            // Add playerId value to Header
            context.Request.Headers.TryAdd("playerId", playerId);

            await next(context);
        }
    }
}