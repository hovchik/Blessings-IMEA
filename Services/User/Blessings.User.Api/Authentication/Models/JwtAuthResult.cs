using System.Text.Json.Serialization;

namespace Blessings.User.Api.Authentication.Models;

public class JwtAuthResult
{

    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refreshToken")]
    public RefreshToken RefreshToken { get; set; }
}