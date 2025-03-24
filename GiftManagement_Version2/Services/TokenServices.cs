using FluentValidation;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GiftManagement_Version2.Services
{
    public interface ITokenServices
    {
        Task<TokenModel> GenerateToken(User user);
        Task<IActionResult> RenewToken(TokenModel model);
    }
    public class TokenServices : ITokenServices
    {
        private readonly MyDBContext _dbContext;
        private readonly IConfiguration _config;
        private readonly IValidator<TokenModel> _tokenValidator;

        public TokenServices(MyDBContext dbContext, IConfiguration config, IValidator<TokenModel> tokenValidator)
        {
            _dbContext = dbContext;
            _config = config;
            _tokenValidator = tokenValidator;
        }

        public async Task<TokenModel> GenerateToken(User user)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new();
            IConfigurationSection jwtToken = _config.GetSection("Jwt");

            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(jwtToken["SecretKey"]);

            SecurityTokenDescriptor tokenDescription = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256)
            };

            SecurityToken token = jwtTokenHandler.CreateToken(tokenDescription);
            string accessToken = jwtTokenHandler.WriteToken(token);
            string refreshToken = GenerateRefreshToken();

            RefreshToken refreshTokenEntity = new()
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = user.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IsUseAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddHours(1),
            };

            await _dbContext.AddAsync(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public string GenerateRefreshToken()
        {
            byte[] randomString = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomString);
                return Convert.ToBase64String(randomString);
            }
        }

        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            TokenValidator validator = new(_dbContext, _config);
            FluentValidation.Results.ValidationResult validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return new JsonResult(new
                {
                    Result = false,
                    Message = validationResult.Errors.First().ErrorMessage
                })
                { StatusCode = StatusCodes.Status200OK };
            }

            RefreshToken? storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == model.RefreshToken);
            storedToken.IsRevoked = true;
            storedToken.IsUsed = true;
            _dbContext.Update(storedToken);
            await _dbContext.SaveChangesAsync();

            User? user = await _dbContext.Users.SingleOrDefaultAsync(nd => nd.Id == storedToken.UserId);
            TokenModel token = await GenerateToken(user);

            return new JsonResult(new
            {
                Result = true,
                Message = "Renew token success",
                Data = token
            })
            { StatusCode = StatusCodes.Status200OK };
        }
    }
}
