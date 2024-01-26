using AutoMapper;
using AutoMapper.QueryableExtensions;
using market.Data.Repository;
using market.Exceptions;
using market.Extensions;
using market.Models.Domain;
using market.Models.DTO.BaseDto;
using market.Models.DTO.File;
using market.Models.DTO.Product;
using market.Models.Enum;
using market.Services.ProductService.Exceptions;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace market.Services.ProductService;

public class ProductService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<ProductTag> _productTagRepository;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IRepository<Brand> _brandRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IWorkContext _workContext;
    private readonly FileService.FileService _fileService;
    private readonly IRepository<ProductImage> _productImageRepository;
    private readonly PanelService _panelService;


    public ProductService(
        IMapper mapper,
        IRepository<Product> productRepository,
        IRepository<ProductTag> productTagRepository,
        IWorkContext workContext,
        IRepository<Tag> tagRepository,
        IRepository<Category> categoryRepository,
        IRepository<Brand> brandRepository,
        FileService.FileService fileService,
         IRepository<ProductImage> productImageRepository,
         PanelService panelService)
    {
        _mapper = mapper;
        _productRepository = productRepository;
        _productTagRepository = productTagRepository;
        _workContext = workContext;
        _tagRepository = tagRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _fileService = fileService;
        _productImageRepository = productImageRepository;
        _panelService = panelService;
    }

    public async Task AddProduct(AddProductInput input, CancellationToken cancellationToken)
    {
        var panelId = await _panelService.GetPanelId(cancellationToken);

        var brand = await _brandRepository.TableNoTracking.SingleOrDefaultAsync(
            x => x.Uuid == input.BrandGuid,
            cancellationToken
        );

        if (brand is null)
            throw new BadRequestException();

        var category = await _categoryRepository.TableNoTracking.SingleOrDefaultAsync(
            x => x.Uuid == input.CategoryGuid,
            cancellationToken
        );

        if (category is null)
            throw new BadRequestException();

        if (
            input.ProductImages is null
            || input.ProductImages.Count(x => x.Type == ProductImageType.Cover) != 1
        )
            throw new BadRequestException();

        var product = _mapper.Map<Product>(input);

        product.PanelId = panelId;
        product.BrandId = brand.Id;
        product.CategoryId = category.Id;

        product.Images = input.ProductImages
            .Select(
                x =>
                    new ProductImage
                    {
                        FileId = x.FileId,
                        ProductId = product.Id,
                        Type = x.Type
                    }
            )
            .ToList();

        await _productRepository.AddAsync(product, cancellationToken);

        var tags = await _tagRepository.TableNoTracking
            .Where(x => input.TagIds.Contains(x.Uuid))
            .ToListAsync(cancellationToken);

        if (input.TagIds.Count != tags.Count)
            throw new BadRequestException();

        var productTags = tags.Select(
                tag => new ProductTag { ProductId = product.Id, TagId = tag.Id }
            )
            .ToList();

        await _productTagRepository.AddRangeAsync(productTags, cancellationToken);
    }

    public async Task UpdateProduct(
        Guid productGuid,
        UpdateProductInput input,
        CancellationToken cancellationToken
    )
    {
        var panelId = await _panelService.GetPanelId(cancellationToken);
        var product = await _productRepository.Table.SingleOrDefaultAsync(
            x => x.Uuid == productGuid,
            cancellationToken
        );

        if (product is null)
            throw new ProductNotFoundException();

        if (product.PanelId != panelId)
            throw new ForbiddenException();

        var brand = await _brandRepository.TableNoTracking.SingleOrDefaultAsync(
            x => x.Uuid == input.BrandGuid,
            cancellationToken
        );

        if (brand is null)
            throw new BadRequestException();

        var category = await _categoryRepository.TableNoTracking.SingleOrDefaultAsync(
            x => x.Uuid == input.CategoryGuid,
            cancellationToken
        );

        if (category is null)
            throw new BadRequestException();

        product.Name = input.Name;
        product.Description = input.Description;
        product.Detail = input.Detail;
        product.Price = input.Price;
        product.Quantity = input.Quantity;
        product.Status = input.Status;
        product.BrandId = brand.Id;
        product.CategoryId = category.Id;
        product.DiscountPrice = input.DiscountPrice;

        foreach (var tagId in input.TagIds)
        {
            var tag = await _tagRepository.TableNoTracking.SingleOrDefaultAsync(
                x => x.Uuid == tagId.TagUuid,
                cancellationToken
            );

            if (tag is null)
                throw new BadRequestException();

            if (tagId.State == UpdateState.Added)
            {
                product.ProductTags.Add(new ProductTag { ProductId = product.Id, TagId = tag.Id });
                continue;
            }

            var productTag = await _productTagRepository.TableNoTracking.SingleOrDefaultAsync(
                x => x.TagId == tag.Id && x.ProductId == product.Id,
                cancellationToken
            );

            if (productTag is null)
                throw new BadRequestException();

            product.ProductTags.Remove(productTag);
        }

        await _productRepository.UpdateAsync(product, cancellationToken);
    }

    public async Task<FilteredResult<GetProductShortResult>> GetAllProductsByCustomers(
        GetAllProductsQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        var productQuery = _productRepository.TableNoTracking.Where(
            x => x.Status == ProductStatus.Purchasable
        );

        if (!queryParams.Title.IsNullOrEmpty())
            productQuery = productQuery.Where(x => x.Name.Contains(queryParams.Title));

        if (queryParams.CategoryId.HasValue)
            productQuery = productQuery.Where(x => x.CategoryId == queryParams.CategoryId);

        if (queryParams.BrandId.HasValue)
            productQuery = productQuery.Where(x => x.BrandId == queryParams.BrandId);

        if (queryParams.IsDiscount)
            productQuery = productQuery.Where(x => x.DiscountPrice != null);

        productQuery = queryParams.Order switch
        {
            GetAllProductsOrder.Newest => productQuery.OrderByDescending(x => x.CreateMoment),
            GetAllProductsOrder.Cheapest => productQuery.OrderBy(x => x.Price),
            GetAllProductsOrder.Discounted
                => productQuery.OrderBy(x => x.DiscountPrice != null),
            _ => productQuery
        };

        var products = await productQuery
            .ProjectTo<GetProductShortResult>(_mapper.ConfigurationProvider)
            .ExecuteWithPaginationAsync(queryParams, cancellationToken);

        foreach (var product in products.Data)
        {
            if (product.Cover is null)
                continue;
            var url = await _fileService.GetFileUrl(product.Cover.FileId, cancellationToken);

            product.Cover.Url = url.Url;
        }

        return products;
    }

    public async Task<GetProductResult> GetProductByCustomer(
        Guid productGuid,
        CancellationToken cancellationToken
    )
    {
        var product = await _productRepository.TableNoTracking
            .Where(x => x.Uuid == productGuid && x.Status == ProductStatus.Purchasable)
            .Include(x => x.Images)!
            .ThenInclude(x => x.File)
            .Include(x => x.ProductTags)
            .SingleOrDefaultAsync(cancellationToken);

        if (product is null)
            throw new ProductNotFoundException();

        var galleryFiles = product.Images?.Where(x => x.Type == ProductImageType.Gallery).Select(x => x.File).ToList();

        var result = _mapper.Map<GetProductResult>(product);

        if (galleryFiles.IsNullOrEmpty() is false)
            result.GalleryImages = _mapper.Map<IList<FileDto>>(galleryFiles);

        if (result.Cover is not null)
            result.Cover.Url = (
                await _fileService.GetFileUrl(result.Cover.FileId, cancellationToken)
            ).Url;

        if (result.GalleryImages is not null)
        {
            foreach (var image in result.GalleryImages)
            {
                image.Url = (await _fileService.GetFileUrl(image.FileId, cancellationToken)).Url;
            }
        }

        return result;
    }

    public async Task<FilteredResult<GetPanelAllProductsResult>> GetPanelAllProducts(
        PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        var products = await _productRepository.TableNoTracking
            .ProjectTo<GetPanelAllProductsResult>(_mapper.ConfigurationProvider)
            .ExecuteWithPaginationAsync(queryParams, cancellationToken);

        foreach (var product in products.Data)
        {
            if (product.Cover is null)
                continue;
            var url = await _fileService.GetFileUrl(product.Cover.FileId, cancellationToken);

            product.Cover.Url = url.Url;
        }

        return products;
    }

    public async Task<GetPanelProductResult> GetPanelProduct(
        Guid productGuid,
        CancellationToken cancellationToken
    )
    {
        var product = await _productRepository.TableNoTracking
            .Where(x => x.Uuid == productGuid)
            .Include(x => x.Images)!
            .ThenInclude(x => x.File)
            .Include(x => x.ProductTags)
            .SingleOrDefaultAsync(cancellationToken);

        if (product is null)
            throw new ProductNotFoundException();

        var galleryFiles = product.Images?.Where(x => x.Type == ProductImageType.Gallery).Select(x => x.File).ToList();

        var result = _mapper.Map<GetPanelProductResult>(product);

        if (galleryFiles.IsNullOrEmpty() is false)
            result.GalleryImages = _mapper.Map<IList<FileDto>>(galleryFiles);

        if (result.Cover is not null)
            result.Cover.Url = (
                await _fileService.GetFileUrl(result.Cover.FileId, cancellationToken)
            ).Url;

        if (result.GalleryImages is not null)
        {
            foreach (var image in result.GalleryImages)
            {
                image.Url = (await _fileService.GetFileUrl(image.FileId, cancellationToken)).Url;
            }
        }

        return result;
    }
}