using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechnicalTestApi.Data;
using TechnicalTestApi.Entities;
using TechnicalTestApi.Middleware;
using TechnicalTestApi.Registers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .RegisterCors()
    .RegisterSwagger()
    .IdentityServices(builder.Configuration)
    .ApplicationServices(builder.Configuration, builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseSwaggerDoc();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCORS();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<ApplicationDataContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();

    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager, builder.Configuration);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
