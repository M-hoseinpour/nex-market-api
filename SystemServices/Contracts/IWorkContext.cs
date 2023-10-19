using System.Security.Claims;
using market.Models.Enum;

namespace market.SystemServices.Contracts;

public interface IWorkContext
{
    int GetUserId();
    UserType GetUserType();
    bool IsAuthenticated();
    string GetTokenFromHeader();
    Claim GetCurrentUserTypeClaim();
    Claim GetCurrentUserIdClaim();
}