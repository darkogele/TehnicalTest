using Microsoft.AspNetCore.Identity;

namespace TechnicalTestApi.Entities;

public class Role : IdentityRole<int>
{
    public List<UserRole>? UserRoles { get; set; }
}