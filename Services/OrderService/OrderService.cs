using AutoMapper;
using AutoMapper.QueryableExtensions;
using market.Data.Repository;
using market.Exceptions;
using market.Extensions;
using market.Models.Domain;
using market.Models.DTO.BaseDto;
using market.Models.DTO.Order;
using market.Models.Enum;
using market.Services.OrderService.Exceptions;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;

namespace market.Services.OrderService;

public class OrderService
{
    private readonly IWorkContext _workContext;
    private readonly IRepository<Address> _addressRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<CartItem> _cartItemRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IWorkContext workContext,
        IRepository<Address> addressRepository,
        IRepository<Product> productRepository,
        IRepository<Order> orderRepository,
        IRepository<CartItem> cartItemRepository, IMapper mapper)
    {
        _workContext = workContext;
        _addressRepository = addressRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
    }

    public async Task AddOrder(AddOrderInput input, CancellationToken cancellationToken)
    {
        var customerId = _workContext.GetCustomerId();

        var address = await _addressRepository.TableNoTracking.FirstOrDefaultAsync(
            x => x.Id == input.AddressId,
            cancellationToken
        );

        if (address is null || address.CustomerId != customerId)
            throw new BadRequestException();

        var products = await _productRepository.TableNoTracking
            .Where(x => input.OrderDetails.Select(a => a.ProductId).Contains(x.Id))
            .ToListAsync(cancellationToken);

        var order = new Order
        {
            CustomerId = customerId,
            AddressId = input.AddressId,
            Status = OrderStatus.Pending,
            OrderDetails = new List<OrderDetail>()
        };

        foreach (var orderDetail in input.OrderDetails)
        {
            order.OrderDetails.Add(
                new OrderDetail
                {
                    Quantity = orderDetail.Quantity,
                    ProductId = orderDetail.ProductId,
                    Price = products.Single(x => x.Id == orderDetail.ProductId).Price
                }
            );
        }

        await _orderRepository.AddAsync(order, cancellationToken);

        var cartItems = await _cartItemRepository.Table
            .Where(x => x.CustomerId == customerId)
            .ToListAsync(cancellationToken);

        await _cartItemRepository.DeleteRangeAsync(cartItems, cancellationToken);
    }

    public async Task<FilteredResult<GetOrderShortResult>> GetOrders(
        GetOrdersQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        var customerId = _workContext.GetCustomerId();
        var orderQuery = _orderRepository.TableNoTracking
            .Where(x => x.CustomerId == customerId);

        if (queryParams.Status.HasValue)
            orderQuery = orderQuery.Where(x => x.Status == queryParams.Status);

        return await orderQuery
            .OrderByDescending(x => x.CreateMoment)
            .ProjectTo<GetOrderShortResult>(_mapper.ConfigurationProvider)
            .ExecuteWithPaginationAsync(queryParams, cancellationToken);
    }

    public async Task<GetOrderFullResult> GetOrder(Guid orderGuid, CancellationToken cancellationToken)
    {
        var customerId = _workContext.GetCustomerId();

        var order = await _orderRepository.TableNoTracking
            .Where(x => x.Uuid == orderGuid && x.CustomerId == customerId)
            .ProjectTo<GetOrderFullResult>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
            throw new OrderNotFoundException();

        return order;
    }
}
