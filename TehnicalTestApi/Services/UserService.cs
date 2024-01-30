using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechnicalTestApi.Dtos;
using TechnicalTestApi.Entities;
using TechnicalTestApi.Services.Contracts;

namespace TechnicalTestApi.Services;

public class UserService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    TokenService tokenService,
    IConfiguration configuration) : IUserService
{
    public async Task<Result<UserDto>> Login(LoginDto loginDto, CancellationToken ct)
    {
        // Find user by UserName
        var user = await userManager.FindByNameAsync(loginDto.UserName);
            //.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName, ct);
        if (user == null) return Result<UserDto>.Failure("Unauthorized");

        // Check if user Password is correct
        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
            return Result<UserDto>.Failure("Invalid Credentials Unauthorized");

        // Get refresh token config from app-settings.json and generate refresh token
        var refreshTokenConfig = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"],
            out var refreshTokenValidityInDays);
        if (!refreshTokenConfig) return Result<UserDto>.Failure("Refresh token config is false");

        user.RefreshToken = tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

        // Update user in db
        await userManager.UpdateAsync(user);

        var userDto = UserDto.FromUser(user, await tokenService.CreateToken(user), user.RefreshToken);

        return Result<UserDto>.Success(userDto);
    }
}