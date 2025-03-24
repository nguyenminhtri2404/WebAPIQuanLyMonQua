using GiftManagement_Version2.Services;
using Microsoft.AspNetCore.Mvc;

namespace GiftManagement_Version2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices orderServices;

        public OrderController(IOrderServices orderServices)
        {
            this.orderServices = orderServices;
        }

        //Checkout
        [HttpPost("Checkout/{userId}")]
        public IActionResult Checkout(int userId)
        {
            string? result = orderServices.Checkout(userId);
            if (result == null)
            {
                return Ok("Order placed successfully.");
            }
            return BadRequest(result);
        }

        //GiftToUser
        [HttpPost("GiftToUser")]
        public IActionResult GiftToUser(int userId, int giftId, int quantity)
        {
            string? result = orderServices.GiftToUser(userId, giftId, quantity);
            if (result == null)
            {
                return Ok("Gift sent successfully.");
            }
            return BadRequest(result);
        }

        //GiftTopUser
        [HttpPost("GiftTopUser")]
        public IActionResult GiftTopUser(int giftId)
        {
            string? result = orderServices.GiftToTopUsers(giftId);
            if (result == null)
            {
                return Ok("Gift sent successfully.");
            }
            return BadRequest(result);
        }


        //Get all orders
        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders(int userID)
        {
            return Ok(orderServices.GetOrderHistory(userID));
        }
    }
}
