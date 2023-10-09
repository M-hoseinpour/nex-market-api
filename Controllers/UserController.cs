using market.Models.Domain;
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

    [HttpGet]

    public async Task<User> GetUsers()
    {
        // return await _userService.GetUsers();
        return null;
    }
}
