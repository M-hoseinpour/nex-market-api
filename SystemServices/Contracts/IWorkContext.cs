using System.Security.Claims;

namespace market.SystemServices.Contracts;

public interface IWorkContext
{
    int GetUserId();
    int GetRoleId();
    bool IsAuthenticated();
    string GetTokenFromHeader();
    Claim GetCurrentRoleIdClaim();
    Claim GetCurrentUserIdClaim();
}