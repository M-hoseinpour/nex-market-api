using market.Models.DTO.BaseDto;

namespace market.Models.DTO.Category;

public class GetCategoriesQueryParams : PaginationQueryParams
{
    public bool? OnlyParentCategories { get; set; }
    public Guid? ParentCategoryUuid { get; set; }
}