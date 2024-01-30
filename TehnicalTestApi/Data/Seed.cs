using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechnicalTestApi.Entities;

namespace TechnicalTestApi.Data;

public static class Seed
{
    public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager,
        IConfiguration config)
    {
        // Seed Roles
        if (!await roleManager.Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new() { Name = RoleNames.Employee },
                new() { Name = RoleNames.Admin }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }

        // Seed Users
        if (!await userManager.Users.AnyAsync())
        {
            var userCentennials = new
            {
                email = config["SeedAdminUser:email"],
                userName = config["SeedAdminUser:userName"],
                password = config["SeedAdminUser:password"]
            };

            if (userCentennials.email is null) return;

            var admin = new User { UserName = userCentennials.userName, Email = userCentennials.email };

            await userManager.CreateAsync(admin, userCentennials.password!);
            await userManager.AddToRoleAsync(admin, RoleNames.Admin);
        }

        var faker = new Faker<User>()
            .RuleFor(p => p.UserName, f => f.Person.UserName)
            .RuleFor(p => p.DateOfBirth, f => f.Person.DateOfBirth)
            .RuleFor(p => p.Email, f => f.Person.Email)
            .RuleFor(p => p.FirstName, f => f.Person.FirstName)
            .RuleFor(p => p.LastName, f => f.Person.LastName)
            .RuleFor(p => p.Title, f => f.PickRandom("Mr.", "Ms.", "Mrs."))
            .RuleFor(p => p.Address, f => f.Address.FullAddress())
            .RuleFor(p => p.Country, f => f.Address.Country())
            .RuleFor(p => p.Bio, f => f.Lorem.Sentence())
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
            .RuleFor(p => p.Rating, f => f.Random.Double(1, 5));

        foreach (var user in faker.Generate(20))
        {
            await userManager.CreateAsync(user);
            await userManager.AddToRoleAsync(user, RoleNames.Employee);
        }
    }
}