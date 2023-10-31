using market.Models.Domain;
using market.Models.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
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

    [HttpGet("profile")]
    public async Task<ProfileBriefResponse> GetBriefProfile(CancellationToken cancellationToken)
    {
        return await _userService.GetBriefProfile(cancellationToken: cancellationToken);
    }

    [HttpPatch("profile/{field}")]
    public async Task UpdateProfileField(
        string field,
        UpdateFieldInput input,
        CancellationToken cancellationToken
)
    {
        await _userService.UpdateProfileField(
            field: field,
            input: input,
            cancellationToken: cancellationToken
        );
    }

    [HttpPost("profile")]
    public async Task<ProfileBriefResponse> CompleteProfile(
    ProfileInput input,
    CancellationToken cancellationToken
)
    {
        return await _userService.CompleteProfile(input: input, cancellationToken: cancellationToken);
    }
}
