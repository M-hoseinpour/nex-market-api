namespace market.Models.DTO.Banner;

public class BannerInput
{
    public required string Title { get; set; }
    public Guid FileId { get; set; }
}