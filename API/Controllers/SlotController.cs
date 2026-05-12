using Application;
using Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlotController(ISlotService slotService) : SpaceGoBaseController
    {
        [HttpPost("spin")]
        [Authorize]
        public async Task<BetResponse> Spin(BetRequest betRequest)
        {
            var playerId = GetPlayer();
            var response = await slotService.Spin(playerId, betRequest.Bet);
            return response;
        }
    }
}
