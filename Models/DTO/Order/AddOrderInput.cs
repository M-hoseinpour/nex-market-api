namespace market.Models.DTO.Order;

public class AddOrderInput
{
    public int AddressId { get; set; }
    public IList<OrderDetailInput> OrderDetails { get; set; }
}