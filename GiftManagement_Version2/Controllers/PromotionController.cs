using GiftManagement_Version2.Models;
using GiftManagement_Version2.Services;
using Microsoft.AspNetCore.Mvc;

namespace GiftManagement_Version2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionServices promotionServices;

        public PromotionController(IPromotionServices promotionServices)
        {
            this.promotionServices = promotionServices;
        }

        [HttpPost]
        public IActionResult CreatePromotion(PromotionRequest request)
        {
            string? result = promotionServices.CreatePromotion(request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpGet("{id}")]
        public IActionResult GetPromotion(int id)
        {
            PromotionRespone promotion = promotionServices.GetPromotion(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return Ok(promotion);
        }

        [HttpGet]
        public IActionResult GetAllPromotion()
        {
            List<PromotionRespone> promotions = promotionServices.GetAllPromotion();
            return Ok(promotions);
        }

        [HttpPut("UpdatePromotion/{id}")]
        public IActionResult UpdatePromotion(int id, [FromForm] PromotionRequest request)
        {
            string? result = promotionServices.UpdatePromotion(id, request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpDelete("DeletePromotion/{id}")]
        public IActionResult DeletePromotion(int id)
        {
            string? result = promotionServices.DeletePromotion(id);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpGet("GetPromotionByMainGift/{mainGiftId}")]
        public IActionResult GetPromotionByMainGift(int mainGiftId)
        {
            List<PromotionRespone> promotions = promotionServices.GetPromotionByMainGift(mainGiftId);
            return Ok(promotions);
        }
    }
}
