using GiftManagement_Version2.Data;
using GiftManagement_Version2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftManagement_Version2.Services
{
    public interface IAuthServices
    {
        Task<TokenModel> LoginAsync(LoginRequest request);
        Task<IActionResult> RenewTokenAsync(TokenModel model);
    }
    public class AuthServices : IAuthServices
    {
        private readonly MyDBContext dbContext;
        private readonly ITokenServices tokenServices;

        public AuthServices(MyDBContext dbContext, ITokenServices tokenServices)
        {
            this.dbContext = dbContext;
            this.tokenServices = tokenServices;
        }

        public async Task<TokenModel> LoginAsync(LoginRequest request)
        {
            User? user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return null;
            }

            return await tokenServices.GenerateToken(user);
        }
        public async Task<IActionResult> RenewTokenAsync(TokenModel model)
        {
            return await tokenServices.RenewToken(model);
        }

    }
}
