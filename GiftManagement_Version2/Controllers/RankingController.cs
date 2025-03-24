using GiftManagement_Version2.Services;
using Microsoft.AspNetCore.Mvc;

namespace GiftManagement_Version2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        private readonly IRankingServices rankingServices;

        public RankingController(IRankingServices rankingServices)
        {
            this.rankingServices = rankingServices;
        }

        [HttpGet("GetRankings")]
        public IActionResult GetRankings()
        {
            return Ok(rankingServices.GetRankings());
        }

        [HttpPost("UpdateRanking")]
        public IActionResult UpdateRanking()
        {
            string? result = rankingServices.UpdateRanking();
            if (result == null)
            {
                return Ok("Ranking updated successfully.");
            }
            return BadRequest(result);
        }

        //Lấy danh sách user có điểm tích lũy và xếp hạng của họ theo tháng
        [HttpGet("GetRankingByMonth")]
        public IActionResult GetRankingByMonth(int month, int year)
        {
            if (month < 1 || month > 12)
            {
                return BadRequest("Invalid month.");
            }

            return Ok(rankingServices.GetRankingsByMonth(month, year));
        }

    }
}
