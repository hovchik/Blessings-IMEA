using Blessings.Contract.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blessings.Contract.TokenValidation;

public static class TokenValidator
{
    public static TokenResponse? ValidateToken(string token, AppSetting secret)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret.JwtTokenConfig.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = secret.JwtTokenConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = secret.JwtTokenConfig.Audience,
                ValidAlgorithms = new List<string>() { SecurityAlgorithms.HmacSha256Signature },
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var role = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value);
            // return user id from JWT token if validation successful
            return new TokenResponse
            {
                UserId = userId,
                Role = role,
            };
        }
        catch
        {
            return null;
        }
    }
}