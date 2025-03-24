using GiftManagement_Version2.Models;
using GiftManagement_Version2.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GiftManagement_Version2.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserServices userServices)
        {
            //Lấy claim từ request
            Claim? claim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            //Lấy quyền cần xác thực
            AuthorizeAttribute? atttribute = context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>();
            string roleRequired = atttribute?.Roles ?? "";

            //Trường hợp không cần xác thực quyền
            if (string.IsNullOrEmpty(roleRequired))
            {
                await next(context);
                return;
            }

            if (claim != null)
            {
                int userdId = int.Parse(claim.Value);
                UserRespone user = userServices.GetProfile(userdId, context);

                if (user != null)
                {
                    //Kiểm tra quyền truy cập
                    bool isAuthorized = user.Permissions.Any(p => roleRequired.Contains(p));

                    if (isAuthorized)
                    {
                        List<Claim> claims = new()
                    {
                        new Claim(ClaimTypes.Role, roleRequired)
                    };
                        ClaimsIdentity claimsIdentity = new(claims, "Custom");
                        context.User.AddIdentity(claimsIdentity);
                        await next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("Access Denied");
                    }
                }
            }
            await next(context);
        }
    }

}
