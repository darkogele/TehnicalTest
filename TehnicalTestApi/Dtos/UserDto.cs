using TechnicalTestApi.Entities;

namespace TechnicalTestApi.Dtos;

public record UserDto(
    int Id,
    string Token,
    string UserName,
    string Email,
    string? PhoneNumber,
    string RefreshToken,
    DateTime? RefreshTokenExpiryTime)
{
    public static UserDto FromUser(User user, string token, string refreshToken) =>
        new(user.Id,
            token,
            user.UserName!,
            user.Email!,
            user.PhoneNumber,
            refreshToken,
            user.RefreshTokenExpiryTime);
}