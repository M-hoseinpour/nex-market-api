using market.Models.DTO.BaseDto;
using market.Models.DTO.Order;
using market.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers;

[ApiController]
[Route(template: "api/orders")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task AddOrder(AddOrderInput input, CancellationToken cancellationToken)
    {
        await _orderService.AddOrder(input: input, cancellationToken: cancellationToken);
    }

    [HttpGet]
    public async Task<FilteredResult<GetOrderShortResult>> GetOrders(
        [FromQuery] GetOrdersQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _orderService.GetOrders(
            queryParams: queryParams,
            cancellationToken: cancellationToken
        );
    }

    [HttpGet(template: "{id:guid}")]
    public async Task<GetOrderFullResult> GetOrder(Guid id, CancellationToken cancellationToken)
    {
        return await _orderService.GetOrder(orderGuid: id, cancellationToken: cancellationToken);
    }
}
