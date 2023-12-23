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
        [FromQuery] PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _productService.GetAllProductsByCustomers(
            queryParams: queryParams,
            cancellationToken: cancellationToken
        );
    }

    [HttpGet(template: "{uuid:guid}")]
    public async Task<GetProductResult> GetProduct(Guid uuid, CancellationToken cancellationToken)
    {
        return await _productService.GetProductByCustomer(
            productGuid: uuid,
            cancellationToken: cancellationToken
        );
    }

    [HttpPost("{uuid:guid}/reviews")]
    public async Task AddReview(Guid uuid, ReviewInput input, CancellationToken cancellationToken)
    {
        await _reviewService.AddReview(
            productGuid: uuid,
            input: input,
            cancellationToken: cancellationToken
        );
    }

    [HttpGet("{uuid:guid}/reviews")]
    public async Task<FilteredResult<ReviewResult>> GetReviews(
        Guid uuid,
        PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _reviewService.GetReviews(
            productGuid: uuid,
            queryParams: queryParams,
            cancellationToken: cancellationToken
        );
    }
}
