using market.Models.Domain;
using market.Models.DTO.BaseDto;
using market.Models.DTO.Product;
using market.Models.DTO.Review;
using market.Services;
using market.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers;

[ApiController]
[Route(template: "api/products")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly ReviewService _reviewService;

    public ProductController(ProductService productService, ReviewService reviewService)
    {
        _productService = productService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<FilteredResult<GetProductShortResult>> GetAllProducts(
        [FromQuery] GetAllProductsQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _productService.GetAllProductsByCustomers(
            queryParams: queryParams,
            cancellationToken: cancellationToken
        );
    }

    [HttpGet(template: "{id:guid}")]
    public async Task<GetProductResult> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        return await _productService.GetProductByCustomer(
            productGuid: id,
            cancellationToken: cancellationToken
        );
    }

    [HttpPost("{id:guid}/reviews")]
    public async Task AddReview(Guid id, ReviewInput input, CancellationToken cancellationToken)
    {
        await _reviewService.AddReview(
            productGuid: id,
            input: input,
            cancellationToken: cancellationToken
        );
    }

    [HttpGet("{id:guid}/reviews")]
    public async Task<FilteredResult<ReviewResult>> GetReviews(
        Guid id,
        PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _reviewService.GetReviews(
            productGuid: id,
            queryParams: queryParams,
            cancellationToken: cancellationToken
        );
    }
}
