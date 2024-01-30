using TechnicalTestApi.Dtos;
using TechnicalTestApi.Services.Contracts;

namespace TechnicalTestApi.Controllers;

[ApiController]
[Route("api")]
public class UsersController(IUserService userService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("token")]
    [SwaggerOperation(Summary = "Login for jwt token")]
    public async Task<ActionResult> Login(LoginDto loginDto, CancellationToken ct)
    {
        return HandleResult(await userService.Login(loginDto, ct));
    }

    private ActionResult HandleResult<T>(Result<T> result)
    {
        if (!result.IsSuccess) return BadRequest(result.Error);

        if (result.Data == null) return NoContent();

        return Ok(result.Data);
    }
}