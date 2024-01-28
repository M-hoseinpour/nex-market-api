using market.Models.DTO.BaseDto;
using market.Models.DTO.Cart;
using market.Models.DTO.User;
using market.Services;
using market.Services.CartService;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly CartService _cartService;

    public UserController(UserService userService, CartService cartService)
    {
        _userService = userService;
        _cartService = cartService;
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

    [HttpPost("cart")]
    public async Task AddToCart(
        CartDto dto,
        CancellationToken cancellationToken
    )
    {
        await _cartService.AddToCart(dto: dto, cancellationToken: cancellationToken);
    }
    
    [HttpGet("cart")]
    public async Task<FilteredResult<CartResultDto>> GetCart(
        [FromQuery] PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _cartService.GetCart(queryParams: queryParams, cancellationToken: cancellationToken);
    }
    
    [HttpDelete("cart/{id:guid}")]
    public async Task DeleteFromCart(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        await _cartService.DeleteFromCart(cartGuid: id, cancellationToken: cancellationToken);
    }
    
    [HttpPost("cart/submit")]
    public async Task SubmitCart(
        SubmitCartInput input,
        CancellationToken cancellationToken
    )
    {
        await _cartService.SubmitCart(input: input, cancellationToken: cancellationToken);
    }
    
    [HttpGet]
    public async Task<FilteredResult<ProfileBriefResponse>> GetUsers(
        [FromQuery] GetUsersQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _userService.GetUsers(queryParams: queryParams, cancellationToken: cancellationToken);
    }
}