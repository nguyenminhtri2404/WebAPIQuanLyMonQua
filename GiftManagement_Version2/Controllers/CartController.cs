using GiftManagement_Version2.Models;
using GiftManagement_Version2.Services;
using Microsoft.AspNetCore.Mvc;

namespace GiftManagement_Version2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices cartServices;

        public CartController(ICartServices cartServices)
        {
            this.cartServices = cartServices;
        }

        [HttpPost("AddToCart")]
        public IActionResult AddToCart(CartRequest request)
        {
            string? result = cartServices.AddToCart(request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpDelete("{userId}/{giftId}")]
        public IActionResult Delete(int userId, int giftId)
        {
            string? result = cartServices.RemoveItemFromCart(userId, giftId);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpPut("{userId}/{giftId}/{quantity}")]
        public IActionResult UpdateQuantity(int userId, int giftId, int quantity)
        {
            string? result = cartServices.UpdateQuantity(userId, giftId, quantity);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpPut("IncreaseQuantity/{userId}/{giftId}")]
        public IActionResult IncreaseQuantity(int userId, int giftId)
        {
            string? result = cartServices.IncreaseQuantity(userId, giftId);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpPut("DecreaseQuantity/{userId}/{giftId}")]
        public IActionResult DecreaseQuantity(int userId, int giftId)
        {
            string? result = cartServices.DecreaseQuantity(userId, giftId);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpGet("GetCart/{userId}")]
        public IActionResult GetCart(int userId)
        {
            return Ok(cartServices.GetCart(userId));
        }

    }
}
