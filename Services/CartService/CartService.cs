using AutoMapper;
using AutoMapper.QueryableExtensions;
using market.Data.Repository;
using market.Exceptions;
using market.Extensions;
using market.Models.Domain;
using market.Models.DTO.BaseDto;
using market.Models.DTO.Cart;
using market.Services.CartService.Exceptions;
using market.Services.ProductService.Exceptions;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;

namespace market.Services.CartService;

public class CartService
{
    private readonly IRepository<CartItem> _cartRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IWorkContext _workContext;
    private readonly IMapper _mapper;

    public CartService(
        IRepository<CartItem> cartRepository,
        IWorkContext workContext,
        IRepository<Product> productRepository,
        IMapper mapper
    )
    {
        _cartRepository = cartRepository;
        _workContext = workContext;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task AddToCart(CartDto dto, CancellationToken cancellationToken)
    {
        var userId = _workContext.GetUserId();

        var product = await _productRepository
            .TableNoTracking
            .FirstOrDefaultAsync(
                predicate: x => x.Id == dto.ProductId,
                cancellationToken: cancellationToken
            );

        if (product is null)
            throw new ProductNotFoundException();

        if (product.Quantity < dto.Quantity)
            throw new BadRequestException();

        var cart = new CartItem
        {
            UserId = userId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };

        await _cartRepository.AddAsync(entity: cart, cancellationToken: cancellationToken);
    }

    public async Task<FilteredResult<CartDto>> GetCart(
        PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        var userId = _workContext.GetUserId();

        var cart = await _cartRepository
            .TableNoTracking
            .Where(predicate: x => x.UserId == userId)
            .ProjectTo<CartDto>(configuration: _mapper.ConfigurationProvider)
            .ExecuteWithPaginationAsync(
                paginationQueryParams: queryParams,
                cancellationToken: cancellationToken
            );

        return cart;
    }

    public async Task DeleteFromCart(int cartId, CancellationToken cancellationToken)
    {
        var userId = _workContext.GetUserId();

        var cart = await _cartRepository
            .Table
            .FirstOrDefaultAsync(
                predicate: x => x.Id == cartId && x.UserId == userId,
                cancellationToken: cancellationToken
            );

        if (cart is null)
            throw new CartItemNotfoundException();

        await _cartRepository.DeleteAsync(entity: cart, cancellationToken: cancellationToken);
    }
}
