namespace market.Models.DTO.BaseDto;

public interface IPaginationQueryParams
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class PaginationQueryParams : IPaginationQueryParams
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
