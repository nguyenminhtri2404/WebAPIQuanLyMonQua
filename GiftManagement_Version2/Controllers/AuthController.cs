using GiftManagement_Version2.Models;
using GiftManagement_Version2.Services;
using Microsoft.AspNetCore.Mvc;

namespace GiftManagement_Version2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices authServices;
        private readonly IConfiguration config;

        public AuthController(IAuthServices authServices, IConfiguration config)
        {
            this.authServices = authServices;
            this.config = config;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            TokenModel token = await authServices.LoginAsync(request);
            if (token == null)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            return Ok(
            new
            {
                Message = "Authenticate success",
                Data = token
            });
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            IActionResult result = await authServices.RenewTokenAsync(model);
            if (result is JsonResult jsonResult)
            {
                return StatusCode(jsonResult.StatusCode.Value, jsonResult.Value);
            }

            return BadRequest(new { Message = "Something went wrong" });
        }
    }
}
