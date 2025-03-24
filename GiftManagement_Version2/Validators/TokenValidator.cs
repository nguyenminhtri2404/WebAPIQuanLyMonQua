using FluentValidation;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GiftManagement_Version2.Validators
{
    public class TokenValidator : AbstractValidator<TokenModel>
    {
        private readonly MyDBContext dbContext;
        private readonly IConfiguration config;

        public TokenValidator(MyDBContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            this.config = config;

            RuleFor(x => x.AccessToken).Must(IsValidTokenFormat).WithMessage("Invalid token format");
            RuleFor(x => x).Must(CheckAlg).WithMessage("Invalid token algorithm");
            RuleFor(x => x).Must(CheckAccessTokenExpiry).WithMessage("Access token has not yet expired");
            RuleFor(x => x).Must(CheckRefreshTokenExistence).WithMessage("Refresh token does not exist");
            RuleFor(x => x).Must(CheckRefreshTokenUsage).WithMessage("Refresh token has been used or revoked");
            RuleFor(x => x).Must(CheckTokenMatch).WithMessage("Token doesn't match");
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(config.GetSection("Jwt")["SecretKey"]);
            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false,
            };
        }

        private bool IsValidTokenFormat(string accessToken)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new();
            return jwtTokenHandler.CanReadToken(accessToken);
        }

        private bool CheckAlg(TokenModel model)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new();
            TokenValidationParameters tokenParam = GetTokenValidationParameters();

            System.Security.Claims.ClaimsPrincipal tokenVerify = jwtTokenHandler.ValidateToken(model.AccessToken, tokenParam, out SecurityToken? validatedToken);
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                return jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private bool CheckAccessTokenExpiry(TokenModel model)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new();
            TokenValidationParameters tokenParam = GetTokenValidationParameters();

            System.Security.Claims.ClaimsPrincipal tokenVerify = jwtTokenHandler.ValidateToken(model.AccessToken, tokenParam, out SecurityToken? validatedToken);
            long utcExpireDate = long.Parse(tokenVerify.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            DateTime expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
            return expireDate <= DateTime.UtcNow;
        }

        private bool CheckRefreshTokenExistence(TokenModel model)
        {
            RefreshToken? storedToken = dbContext.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
            return storedToken != null;
        }

        private bool CheckRefreshTokenUsage(TokenModel model)
        {
            RefreshToken? storedToken = dbContext.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
            return storedToken != null && !storedToken.IsUsed && !storedToken.IsRevoked;
        }

        private bool CheckTokenMatch(TokenModel model)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new();
            TokenValidationParameters tokenParam = GetTokenValidationParameters();

            System.Security.Claims.ClaimsPrincipal tokenVerify = jwtTokenHandler.ValidateToken(model.AccessToken, tokenParam, out SecurityToken? validatedToken);
            string jti = tokenVerify.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            RefreshToken? storedToken = dbContext.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
            return storedToken != null && storedToken.JwtId == jti;
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            DateTime dateTimeInterval = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
        }
    }
}
