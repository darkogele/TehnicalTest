using System.Text.Json.Serialization;
using TechnicalTestApi.Entities;

namespace TechnicalTestApi.Dtos;

public class UserDto
{
    public int Id { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = default!;

    [JsonPropertyName("expires_at")]
    public string ExpiresAt { get; set; } = default!;

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;


    public static UserDto FromUser(User user, string tokenType, string token, string expiresAt) =>
        new()
        {
            Id = user.Id,
            TokenType = tokenType,
            ExpiresAt = expiresAt,
            AccessToken = token
        };
}