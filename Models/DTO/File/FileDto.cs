using AutoMapper;

namespace market.Models.DTO.File;

public class FileDto
{
    public Guid FileId { get; set; }
    public string Url { get; set; }
}

public class FileDtoMap : Profile
{
    public FileDtoMap()
    {
        CreateMap<Data.Domain.File, FileDto>()
            .ForMember(
                destinationMember: x => x.FileId,
                memberOptions: x => x.MapFrom(source => source.Id)
            );
    }
}