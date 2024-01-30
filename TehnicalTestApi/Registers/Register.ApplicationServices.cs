using Microsoft.EntityFrameworkCore;
using TechnicalTestApi.Data;
using TechnicalTestApi.Services;
using TechnicalTestApi.Services.Contracts;

namespace TechnicalTestApi.Registers;

public static partial class Register
{
    public static IServiceCollection ApplicationServices(this IServiceCollection services, IConfiguration config,
        IWebHostEnvironment environment)
    {
        // Dotnet build in services
        services.AddControllers();

        services.AddHttpClient();

        services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddHealthChecks();

        // Database configuration
        services.AddDbContext<ApplicationDataContext>(options =>
            options.UseSqlite(config.GetConnectionString("DefaultConnection")));

        // Dependency injection of custom Services
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}