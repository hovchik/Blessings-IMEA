using Blessings.Contract;
using Blessings.User.Api.Authentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Blessings.User.Api.Authentication.Services;

public class JwtAuthManager : IJwtAuthManager
{
    private readonly JwtTokenConfig _jwtTokenConfig;
    private readonly byte[] _secret;

    public JwtAuthManager(JwtTokenConfig jwtTokenConfig)
    {
        _jwtTokenConfig = jwtTokenConfig;
        _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
    }

    public JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now)
    {
        var accessToken = GenerateAccessToken(claims, now, _jwtTokenConfig.AccessTokenExpiration);

        var refreshToken = new RefreshToken
        {
            UserName = username,
            TokenString = GenerateRefreshTokenString(),
            ExpireAt = now.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration)
        };

        return new JwtAuthResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public string GenerateAccessToken(Claim[] claims, DateTime now, double minuteExpiry)
    {
        var shouldAddAudienceClaim =
            string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
        var jwtToken = new JwtSecurityToken(
            _jwtTokenConfig.Issuer,
            shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
            claims,
            expires: now.AddMinutes(minuteExpiry),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret),
                SecurityAlgorithms.HmacSha256Signature));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return accessToken;
    }

    public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now)
    {
        var tokenParts = accessToken.Split(" ");
        if (tokenParts.Length != 2)
        {
            throw new ArgumentException("Invalid Token");
        }
        if (!tokenParts[0].Equals("Bearer", StringComparison.InvariantCultureIgnoreCase))
        {
            throw new ArgumentException("Invalid Token");
        }
        var principal = DecodeJwtToken(tokenParts[1]);
        if (principal?.Identity == null)
        {
            throw new ArgumentException("Invalid Token");
        }
        var userName = principal.Identity.Name;

        return GenerateTokens(userName, principal.Claims.ToArray(), now); // need to recover the original claims
    }

    public ClaimsPrincipal DecodeJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Invalid Token");
        }
        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ValidAudience = _jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(15)
                },
                out var validatedToken);
        var jwtToken = validatedToken as JwtSecurityToken;
        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature) || principal?.Identity == null)
        {
            throw new ArgumentException("Invalid Token");
        }

        return principal;
    }

    private static string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}