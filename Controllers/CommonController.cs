using market.Models.DTO.Banner;
using market.Models.DTO.BaseDto;
using market.Models.DTO.Brand;
using market.Models.DTO.Category;
using market.Services;
using market.Services.BrandService;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers;

[ApiController]
[Route("api/commons")]
public class CommonController : ControllerBase
{
    private readonly BrandService _brandService;
    private readonly CategoryService _categoryService;
    private readonly BannerService _bannerService;

    public CommonController(
        BrandService brandService,
        CategoryService categoryService,
        BannerService bannerService
    )
    {
        _brandService = brandService;
        _categoryService = categoryService;
        _bannerService = bannerService;
    }

    [HttpGet("brands")]
    public async Task<FilteredResult<BrandResult>> GetBrands(
        [FromQuery] PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _brandService.GetBrands(
            queryParams: queryParams,
            cancellationToken: cancellationToken,
            isAllBrands: true
        );
    }

    [HttpGet("categories")]
    public async Task<FilteredResult<CategoryResult>> GetCategories(
        [FromQuery] GetCategoriesQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _categoryService.GetCategories(
            queryParams: queryParams,
            cancellationToken: cancellationToken,
            isAllCategories: true
        );
    }

    [HttpGet("banners")]
    public async Task<FilteredResult<BannerResult>> GetBanners(
        [FromQuery] PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _bannerService.GetBanners(
            queryParams: queryParams,
            cancellationToken: cancellationToken,
            isAllBanners: true
        );
    }
}
