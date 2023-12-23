namespace market.Models.DTO.Review;

public class ReviewInput
{
    public required decimal Rating { get; set; }
    public string? Comment { get; set; }
}