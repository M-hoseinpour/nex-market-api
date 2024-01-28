using AutoMapper;
using market.Models.Domain;

public class AddressResponse
{
    public Guid Uuid { get; set; }
    public required string Location { get; set; }

}

public class AddressResponseMap : Profile
{
    public AddressResponseMap()
    {
        CreateMap<Address, AddressResponse>();
    }
}