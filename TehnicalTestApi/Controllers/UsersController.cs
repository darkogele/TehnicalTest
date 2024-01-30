using TechnicalTestApi.Dtos;
using TechnicalTestApi.Entities;
using TechnicalTestApi.Services.Contracts;

namespace TechnicalTestApi.Controllers;

[ApiController, Authorize]
[Route("api")]
public class UsersController(IUserService userService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("token")]
    [SwaggerOperation(Summary = "Login for jwt token")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken ct)
    {
        return HandleResult(await userService.Login(loginDto, ct));
    }

    [Authorize(Policy = PolicyRoleName.PolicyAdmin)]
    [HttpGet("employee/list/")]
    [SwaggerOperation(Summary = "Get all employees list from db as admin")]
    public async Task<ActionResult<List<EmployeeDto>>> GetEmployeeList(CancellationToken ct)
    {
        return HandleResult(await userService.GetEmployeeList(ct));
    }

    private ActionResult HandleResult<T>(Result<T> result)
    {
        if (!result.IsSuccess) return BadRequest(result.Error);

        if (result.Data == null) return NoContent();

        return Ok(result.Data);
    }
}