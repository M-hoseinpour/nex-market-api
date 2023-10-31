namespace market.Models.DTO.BaseDto;

public class FilteredResult<T>
{
    public long Total { get; set; }
    public IList<T> Data { get; set; } = new List<T>();
}
