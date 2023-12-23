using AutoMapper;

namespace market.Models.DTO.Category;

public class CategoryResult
{
    public required Guid Uuid { get; set; }
    public required string Name { get; set; }
    public CategoryResult? ParentCategory { get; set; }
}

public class CategoryResultMap : Profile
{
    public CategoryResultMap()
    {
        CreateMap<Domain.Category, CategoryResult>()
            .ForMember(des => des.ParentCategory, opt => opt.MapFrom(src => src.ParentCategory));
    }
}
