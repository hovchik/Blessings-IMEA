using Blessings.User.Api.Authentication.Models;
using System.Security.Claims;

namespace Blessings.User.Api.Authentication.Services;

public interface IJwtAuthManager
{
    JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now);
    JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now);
    ClaimsPrincipal DecodeJwtToken(string token);
    string GenerateAccessToken(Claim[] claims, DateTime now, double minuteExpiry);
}