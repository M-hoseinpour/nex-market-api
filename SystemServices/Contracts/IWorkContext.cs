using System.Security.Claims;
using market.Models.Enum;

namespace market.SystemServices.Contracts;

public interface IWorkContext
{
    int GetCustomerId();
    int GetUserId();
    int GetStaffId();
    int GetManagerId();
    int? GetPanelId();
    UserType GetUserType();
    bool IsAuthenticated();
    string GetTokenFromHeader();
    Claim GetCurrentUserTypeClaim();
    Claim GetCurrentUserIdClaim();
    Claim GetCurrentStaffIdClaim();
    Claim GetCurrentCustomerIdClaim();
    Claim GetCurrentManagerIdClaim();
}