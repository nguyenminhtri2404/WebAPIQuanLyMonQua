using GiftManagement_Version2.Models;
using GiftManagement_Version2.Services;
using Microsoft.AspNetCore.Mvc;

namespace GiftManagement_Version2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftServices giftServices;

        public GiftController(IGiftServices giftServices)
        {
            this.giftServices = giftServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGift([FromForm] GiftRequest request)
        {
            string? result = await giftServices.CreateGift(request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpGet("{id}")]
        public IActionResult GetGift(int id)
        {
            GiftRespone gift = giftServices.GetGift(id);
            if (gift == null)
            {
                return NotFound();
            }
            return Ok(gift);
        }

        [HttpGet]
        public IActionResult GetAllGift()
        {
            List<GiftRespone> gifts = giftServices.GetAllGift();
            return Ok(gifts);
        }

        [HttpPut("UpdateGift/{id}")]
        public async Task<IActionResult> UpdateGift(int id, [FromForm] GiftRequest request)
        {
            string? result = await giftServices.UpdateGift(id, request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpDelete("DeleteGift/{id}")]
        public IActionResult DeleteGift(int id)
        {
            string? result = giftServices.DeleteGift(id);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }
    }
}
