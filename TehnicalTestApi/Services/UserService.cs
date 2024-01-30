using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechnicalTestApi.Data;
using TechnicalTestApi.Dtos;
using TechnicalTestApi.Entities;
using TechnicalTestApi.Services.Contracts;

namespace TechnicalTestApi.Services;

public class UserService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    TokenService tokenService,
    ApplicationDataContext dataContext,
    IConfiguration configuration) : IUserService
{
    public async Task<Result<UserDto>> Login(LoginDto loginDto, CancellationToken ct)
    {
        // Find user by UserName
        var user = await userManager.FindByNameAsync(loginDto.UserName);

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
        var tokenData = await tokenService.CreateToken(user);

        var userDto = UserDto.FromUser(user, tokenData.TokenType, tokenData.AccessToken, tokenData.ExpiresAt);

        return Result<UserDto>.Success(userDto);
    }

    public async Task<Result<List<EmployeeDto>>> GetEmployeeList(CancellationToken ct)
    {
        // Here we can add search and pagination in the future
        var employees = await dataContext.Users
            .Where(x => x.UserRoles!.Any(r => r.Role!.Name == RoleNames.Employee))
            .Select(x => new EmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Address,
                Title = x.Title,
                Email = x.Email,
                Country = x.Country,
                DateOfBirth = x.DateOfBirth,
                Bio = x.Bio,
                Rating = x.Rating,
                Image = x.Image
            }).ToListAsync(ct);

        return Result<List<EmployeeDto>>.Success(employees);
    }
}