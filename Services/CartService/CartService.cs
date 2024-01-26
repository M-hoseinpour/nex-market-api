using AutoMapper;
using AutoMapper.QueryableExtensions;
using market.Data.Repository;
using market.Exceptions;
using market.Extensions;
using market.Models.Domain;
using market.Models.DTO.BaseDto;
using market.Models.DTO.Cart;
using market.Models.Enum;
using market.Services.CartService.Exceptions;
using market.Services.ProductService.Exceptions;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace market.Services.CartService;

public class CartService
{
    private readonly IRepository<CartItem> _cartRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IWorkContext _workContext;
    private readonly IMapper _mapper;
    private readonly IRepository<Address> _addressRepository;
    private readonly IRepository<Order> _orderRepository;

    public CartService(
        IRepository<CartItem> cartRepository,
        IWorkContext workContext,
        IRepository<Product> productRepository,
        IMapper mapper, IRepository<Address> addressRepository, IRepository<Order> orderRepository)
    {
        _cartRepository = cartRepository;
        _workContext = workContext;
        _productRepository = productRepository;
        _mapper = mapper;
        _addressRepository = addressRepository;
        _orderRepository = orderRepository;
    }

    public async Task AddToCart(CartDto dto, CancellationToken cancellationToken)
    {
        var customerId = _workContext.GetCustomerId();

        var product = await _productRepository
            .TableNoTracking
            .FirstOrDefaultAsync(
                predicate: x => x.Uuid == dto.ProductUuid,
                cancellationToken: cancellationToken
            );

        if (product is null)
            throw new ProductNotFoundException();

        var productInCart = await _cartRepository.TableNoTracking
             .Include(x => x.Product)
             .Where(p => p.Product.Uuid == product.Uuid)
             .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (productInCart is not null)
        {
            if (productInCart.Quantity < dto.Quantity)
            {
                if (product.Quantity < dto.Quantity)
                    throw new BadRequestException("Not enough product!");
            }
            productInCart.Quantity = dto.Quantity;
            await _cartRepository.UpdateAsync(productInCart, cancellationToken: cancellationToken);
            return;
        }

        if (product.Quantity < dto.Quantity)
            throw new BadRequestException("Not enough product!");

        var cart = new CartItem
        {
            CustomerId = customerId,
            ProductId = product.Id,
            Quantity = dto.Quantity
        };

        await _cartRepository.AddAsync(entity: cart, cancellationToken: cancellationToken);
    }

    public async Task<FilteredResult<CartDto>> GetCart(
        PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        var customerId = _workContext.GetCustomerId();

        var cart = await _cartRepository
            .TableNoTracking
            .Where(predicate: x => x.CustomerId == customerId)
            .ProjectTo<CartDto>(configuration: _mapper.ConfigurationProvider)
            .ExecuteWithPaginationAsync(
                paginationQueryParams: queryParams,
                cancellationToken: cancellationToken
            );

        return cart;
    }

    public async Task DeleteFromCart(Guid cartGuid, CancellationToken cancellationToken)
    {
        var customerId = _workContext.GetCustomerId();

        var cart = await _cartRepository
            .Table
            .Include(x => x.Product)
            .FirstOrDefaultAsync(
                predicate: x => x.Product.Uuid == cartGuid && x.CustomerId == customerId,
                cancellationToken: cancellationToken
            );

        if (cart is null)
            throw new CartItemNotfoundException();

        await _cartRepository.DropAsync(entity: cart, cancellationToken: cancellationToken);
    }

    public async Task SubmitCart(SubmitCartInput input, CancellationToken cancellationToken)
    {
        var customerId = _workContext.GetCustomerId();

        var cartItems = await _cartRepository
            .Table
            .Where(x => x.CustomerId == customerId)
            .Include(x => x.Product)
            .ToListAsync(
                cancellationToken: cancellationToken
            );

        if (cartItems.IsNullOrEmpty())
            throw new CartIsEmptyException();

        var address = await _addressRepository.TableNoTracking.FirstOrDefaultAsync(
            x => x.Uuid == input.AddressUuid,
            cancellationToken
        );

        if (address is null || address.CustomerId != customerId)
            throw new BadRequestException();

        var order = new Order
        {
            CustomerId = customerId,
            AddressId = address.Id,
            Status = OrderStatus.Pending,
            OrderDetails = new List<OrderDetail>()
        };


        foreach (var cartItem in cartItems)
        {
            order.OrderDetails.Add(new OrderDetail
            {
                Quantity = cartItem.Quantity,
                ProductId = cartItem.ProductId,
                Price = cartItem.Product.Price
            });
        }

        await _orderRepository.AddAsync(order, cancellationToken);

        await _cartRepository.DropRangeAsync(cartItems, cancellationToken);
    }
}
