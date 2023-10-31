using AutoMapper;

namespace market.Models.DTO.Tag;

public class TagResponse
{
    public required string Name { get; set; }
}

public class TagResponseMap : Profile
{
    public TagResponseMap()
    {
        CreateMap<Domain.Tag, TagResponse>();
    }
}