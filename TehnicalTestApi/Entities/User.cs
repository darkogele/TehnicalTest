using Microsoft.AspNetCore.Identity;

namespace TechnicalTestApi.Entities;

public class User : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Title { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
    public double Rating { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    //Fk
    public List<UserRole>? UserRoles { get; init; }
}