using System.Security.Claims;
using market.Exceptions;
using market.SystemServices.Contracts;

namespace market.SystemServices;

public class WorkContext : IWorkContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public int GetUserId()
    {
        var userClaim = GetCurrentUserIdClaim();

        if (userClaim is null)
        {
            throw new BadAuthorizationTokenException();
        }

        if (int.TryParse(userClaim.Value, out var userId))
            return userId;

        throw new BadAuthorizationTokenException();
    }

    public int GetRoleId()
    {
        var userClaim = GetCurrentRoleIdClaim();

        if (int.TryParse(userClaim.Value, out var roleId))
            return roleId;

        throw new BadAuthorizationTokenException();
    }

    public bool IsAuthenticated()
    {
        return GetCurrentUserIdClaim()?.Value != null;
    }

    public string GetTokenFromHeader()
    {
        var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        if (authorizationHeader is null)
            throw new AuthorizationTokenNotFoundException();

        return authorizationHeader[7..];
    }

    public Claim GetCurrentUserIdClaim()
    {
        var claimType = "user-id";
        return GetClaim(claimType);
    }

    public Claim GetCurrentRoleIdClaim()
    {
        var claimType = "role-id";
        return GetClaim(claimType);
    }

    private Claim GetClaim(string claimType)
    {
        var claim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == claimType);

        return claim;
    }
}