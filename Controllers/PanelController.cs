using market.Models.DTO.BaseDto;
using market.Models.DTO.Brand;
using market.Models.DTO.Category;
using market.Models.DTO.panel;
using market.Models.DTO.Panel;
using market.Models.DTO.Product;
using market.Services;
using market.Services.BrandService;
using market.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers;

[ApiController]
[Route("api/panels")]
public class PanelController : ControllerBase
{
    private readonly PanelService _panelService;
    private readonly ProductService _productService;
    private readonly BrandService _brandService;
    private readonly CategoryService _categoryService;

    public PanelController(
        PanelService panelService,
        ProductService productService,
        BrandService brandService,
        CategoryService categoryService
    )
    {
        _panelService = panelService;
        _productService = productService;
        _brandService = brandService;
        _categoryService = categoryService;
    }

    [HttpPost]
    //todo panel should be added in manager registration
    public async Task AddPanel(PanelInput panelInput, CancellationToken cancellationToken)
    {
        await _panelService.AddPanel(panelInput, cancellationToken);
    }

    [HttpGet]
    public async Task<GetPanel> GetPanel(CancellationToken cancellationToken)
    {
        return await _panelService.GetPanel(cancellationToken);
    }

    [HttpPost("members")]
    public async Task AddStaff(AddStaffInput addStaffInput, CancellationToken cancellationToken)
    {
        await _panelService.AddStaff(addStaffInput, cancellationToken);
    }

    [HttpGet("members")]
    public async Task<IList<PanelMember>?> GetPanelMembers(
        Guid panelGuid,
        CancellationToken cancellationToken
    )
    {
        return await _panelService.GetPanelMembers(panelGuid, cancellationToken);
    }

    [HttpPost("products")]
    public async Task AddProduct(AddProductInput input, CancellationToken cancellationToken)
    {
        await _productService.AddProduct(input: input, cancellationToken: cancellationToken);
    }

    [HttpPut("products/{uuid:Guid}")]
    public async Task UpdateProduct(
        Guid uuid,
        UpdateProductInput input,
        CancellationToken cancellationToken
    )
    {
        await _productService.UpdateProduct(
            productGuid: uuid,
            input: input,
            cancellationToken: cancellationToken
        );
    }

    [HttpGet("products")]
    public async Task<FilteredResult<GetPanelAllProductsResult>> GetPanelAllProducts(
        [FromQuery] PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _productService.GetPanelAllProducts(
            queryParams: queryParams,
            cancellationToken: cancellationToken
        );
    }

    [HttpGet(template: "products/{uuid:guid}")]
    public async Task<GetPanelProductResult> GetPanelProduct(
        Guid uuid,
        CancellationToken cancellationToken
    )
    {
        return await _productService.GetPanelProduct(
            productGuid: uuid,
            cancellationToken: cancellationToken
        );
    }

    [HttpPost("brands")]
    public async Task AddBrand(BrandInput input, CancellationToken cancellationToken)
    {
        await _brandService.AddBrand(input: input, cancellationToken: cancellationToken);
    }

    [HttpGet("brands")]
    public async Task<FilteredResult<BrandResult>> GetBrands(
        [FromQuery] PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _brandService.GetBrands(
            queryParams: queryParams,
            cancellationToken: cancellationToken
        );
    }

    [HttpDelete("brands/{uuid:guid}")]
    public async Task DeleteBrand(Guid uuid, CancellationToken cancellationToken)
    {
        await _brandService.DeleteBrand(brandGuid: uuid, cancellationToken: cancellationToken);
    }

    [HttpPost("categories")]
    public async Task AddCategory(CategoryInput input, CancellationToken cancellationToken)
    {
        await _categoryService.AddCategory(input: input, cancellationToken: cancellationToken);
    }
    
    [HttpGet("categories")]
    public async Task<FilteredResult<CategoryResult>> GetCategories(
        [FromQuery] GetCategoriesQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _categoryService.GetCategories(
            queryParams: queryParams,
            cancellationToken: cancellationToken
        );
    }
}
