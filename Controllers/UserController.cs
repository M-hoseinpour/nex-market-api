using market.Models.Domain;
using market.Models.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<RegisterResponse> RegisterUser(RegisterInput registerInput, CancellationToken cancellationToken)
    {
        return await _userService.RegisterUser(registerInput, cancellationToken);
    }
    [HttpPost("login")]
    public async Task<RegisterResponse> LoginUser(RegisterInput registerInput, CancellationToken cancellationToken)
    {
        return await _userService.LoginUser(registerInput, cancellationToken);
    }
}
