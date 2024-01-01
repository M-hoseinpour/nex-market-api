using market.Models.DTO.BaseDto;
using market.Models.Enum;

namespace market.Models.DTO.User;

public class GetUsersQueryParams : PaginationQueryParams
{
    public Guid? Uuid { get; set; }
    public UserType? Role { get; set; }
}