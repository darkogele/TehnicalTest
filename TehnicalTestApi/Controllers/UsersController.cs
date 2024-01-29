using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechnicalTestApi.Dtos;

// TechnicalTestApi
namespace TechnicalTestApi.Controllers;

[Route("api/token")]
[ApiController]
public class UsersController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login for jwt token")]
    public async Task<ActionResult> Login(LoginDto loginDto, CancellationToken ct)
    {
        return Ok();
    }
}