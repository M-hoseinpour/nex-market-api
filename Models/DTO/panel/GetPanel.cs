using market.Models.Domain;
using AutoMapper;

namespace market.Models.DTO.panel;

public class GetPanel
{
    public required string Name { get; set; }
}

public class GetPanelMap : Profile
{
    public GetPanelMap()
    {
        CreateMap<Panel, GetPanel>();
    }
}