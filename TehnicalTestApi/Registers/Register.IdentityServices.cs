using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TechnicalTestApi.Data;
using TechnicalTestApi.Entities;
using TechnicalTestApi.Services;

namespace TechnicalTestApi.Registers;

public static partial class Register
{
    public static IServiceCollection IdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.SignIn.RequireConfirmedAccount = false;
        })
            .AddSignInManager<SignInManager<User>>()
            .AddRoleManager<RoleManager<Role>>()
            .AddRoleValidator<RoleValidator<Role>>()
            .AddEntityFrameworkStores<ApplicationDataContext>()
            .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(7));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:TokenKey"]!));

        if (key is null) throw new Exception("Security Key is null Inside Identity Services");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(tokenOptions =>
            {
                tokenOptions.SaveToken = true;
                tokenOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = key
                };

                tokenOptions.Events = new JwtBearerEvents
                {
                    // TODO if need it
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyRoleName.PolicyAdmin, policy =>
                policy.RequireRole(RoleNames.Admin));
        });

        services.AddScoped<TokenService>();

        return services;
    }
}