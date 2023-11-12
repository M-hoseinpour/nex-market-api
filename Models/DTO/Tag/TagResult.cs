using AutoMapper;

namespace market.Models.DTO.Tag;

public class TagResult
{
    public Guid Uuid { get; set; }
    public required string Name { get; set; }
}

public class TagResultMap : Profile
{
    public TagResultMap()
    {
        CreateMap<Domain.Tag, TagResult>();
    }
}