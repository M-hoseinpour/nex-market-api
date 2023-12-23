namespace market.Models.DTO.Category;

public class CategoryInput
{
    public required string Name { get; set; }
    public Guid? ParentUuid { get; set; }
}