namespace market.Models.DTO.Brand;

public class BrandInput
{
    public required string Name { get; set; }
    public Guid? LogoFileId { get; set; }
}