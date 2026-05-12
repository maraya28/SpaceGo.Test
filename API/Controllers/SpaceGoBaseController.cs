using Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SpaceGoBaseController : ControllerBase
    {
        protected string GetPlayer()
        {
            string? id = HttpContext.Request.Headers[Properties.PlayerId];
            var playerId = id ?? string.Empty;
            return playerId;
        }
    }
}