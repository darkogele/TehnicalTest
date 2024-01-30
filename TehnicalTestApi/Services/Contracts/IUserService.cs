using TechnicalTestApi.Core;
using TechnicalTestApi.Dtos;

namespace TechnicalTestApi.Services.Contracts;

public interface IUserService
{
    Task<Result<UserDto>> Login(LoginDto loginDto, CancellationToken ct);
}