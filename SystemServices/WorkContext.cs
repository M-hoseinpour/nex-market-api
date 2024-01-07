using System.Security.Claims;
using market.Exceptions;
using market.Models.Enum;
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

    public int GetCustomerId()
    {
        var customerIdClaim = GetCurrentCustomerIdClaim();

        if (customerIdClaim is null)
        {
            throw new BadAuthorizationTokenException();
        }

        if (int.TryParse(customerIdClaim.Value, out var customerId))
            return customerId;

        throw new BadAuthorizationTokenException();
    }
    public int GetStaffId()
    {
        var userClaim = GetCurrentStaffIdClaim();

        if (userClaim is null)
        {
            throw new BadAuthorizationTokenException();
        }

        if (int.TryParse(userClaim.Value, out var staffId))
            return staffId;

        throw new BadAuthorizationTokenException();
    }
    public int GetManagerId()
    {
        var userClaim = GetCurrentManagerIdClaim();

        if (userClaim is null)
        {
            throw new BadAuthorizationTokenException();
        }

        if (int.TryParse(userClaim.Value, out var managerId))
            return managerId;

        throw new BadAuthorizationTokenException();
    }

    // do not use this directly use GetPanelId of panelService instead 
    public int? GetPanelId()
    {
        var userClaim = GetCurrentPanelIdClaim();

        if (userClaim is null)
        {
            return null;
        }

        if (int.TryParse(userClaim.Value, out var panelId))
            return panelId;

        throw new BadAuthorizationTokenException();
    }

    public UserType GetUserType()
    {
        var userClaim = GetCurrentUserTypeClaim();

        if (Enum.TryParse(userClaim.Value, out UserType userType))
            return userType;

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
    public Claim GetCurrentCustomerIdClaim()
    {
        var claimType = "customer-id";
        return GetClaim(claimType);
    }
    public Claim GetCurrentStaffIdClaim()
    {
        var claimType = "staff-id";
        return GetClaim(claimType);
    }
    public Claim GetCurrentManagerIdClaim()
    {
        var claimType = "manager-id";
        return GetClaim(claimType);
    }

    public Claim GetCurrentPanelIdClaim()
    {
        var claimType = "panel-id";
        return GetClaim(claimType);
    }

    public Claim GetCurrentUserTypeClaim()
    {
        var claimType = "user-type";
        return GetClaim(claimType);
    }

    private Claim GetClaim(string claimType)
    {
        var claim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == claimType);

        return claim;
    }
}