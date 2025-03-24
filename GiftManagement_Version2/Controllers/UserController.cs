using GiftManagement_Version2.Models;
using GiftManagement_Version2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GiftManagement_Version2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices userServices;

        public UserController(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] UserRegisterRequest request)
        {
            string? result = await userServices.Register(request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            UserRespone user = userServices.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            List<UserRespone> users = userServices.GetAllUser();
            return Ok(users);
        }

        [HttpPut("UpdateAccount/{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromForm] UserRegisterRequest request)
        {
            string? result = await userServices.UpdateAccount(id, request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpDelete("DeleteAccount/{id}")]
        public IActionResult DeleteAccount(int id)
        {
            string? result = userServices.DeleteAccount(id);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        //Get profile chỉ user mới xme được thông tin của mình
        [HttpGet("GetProfile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            HttpContext context = HttpContext;
            Claim? claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            return Ok(userServices.GetProfile(int.Parse(claim!.Value), context));
        }
    }
}
