using market.Models.DTO.BaseDto;
using market.Models.DTO.Product;
using market.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task AddProduct(AddProductInput input, CancellationToken cancellationToken)
    {
        await _productService.AddProduct(input, cancellationToken);
    }

    [HttpPut]
    public async Task UpdateProduct(UpdateProductInput input, CancellationToken cancellationToken)
    {
        await _productService.UpdateProduct(input, cancellationToken);
    }

    [HttpGet]
    public async Task<FilteredResult<GetProductShortResult>> GetAllProducts(
        [FromQuery] PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _productService.GetAllProducts(queryParams, cancellationToken);
    }
    
    [HttpGet("{id:int}")]
    public async Task<GetProductResult> GetProduct(
        int id,
        CancellationToken cancellationToken
    )
    {
        return await _productService.GetProduct(id, cancellationToken);
    }
}