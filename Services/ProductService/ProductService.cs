using AutoMapper;
using AutoMapper.QueryableExtensions;
using market.Data.Repository;
using market.Exceptions;
using market.Extensions;
using market.Models.Domain;
using market.Models.DTO.BaseDto;
using market.Models.DTO.Product;
using market.Models.DTO.User;
using market.Models.Enum;
using market.Services.ProductService.Exceptions;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;

namespace market.Services.ProductService;

public class ProductService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<ProductTag> _productTagRepository;
    private readonly IWorkContext _workContext;

    public ProductService(
        IMapper mapper,
        IRepository<Product> productRepository,
        IRepository<ProductTag> productTagRepository,
        IWorkContext workContext
    )
    {
        _mapper = mapper;
        _productRepository = productRepository;
        _productTagRepository = productTagRepository;
        _workContext = workContext;
    }

    public async Task AddProduct(AddProductInput input, CancellationToken cancellationToken)
    {
        var panelId = _workContext.GetPanelId();

        var product = _mapper.Map<Product>(input);

        product.PanelId = panelId;

        await _productRepository.AddAsync(product, cancellationToken);

        var productTags = input.TagIds
            .Where(x => x.State == UpdateState.Added)
            .Select(item => new ProductTag { ProductId = product.Id, TagId = item.TagId })
            .ToList();

        await _productTagRepository.AddRangeAsync(productTags, cancellationToken);
    }

    public async Task UpdateProduct(UpdateProductInput input, CancellationToken cancellationToken)
    {
        var panelId = _workContext.GetPanelId();
        var product = await _productRepository.Table.SingleOrDefaultAsync(
            x => x.Id == input.Id,
            cancellationToken
        );

        if (product is null)
            throw new ProductNotFoundException();

        if (product.PanelId != panelId)
            throw new ForbiddenException();

        product.Name = input.Name;
        product.Description = input.Description;
        product.Detail = input.Detail;
        product.Price = input.Price;
        product.Quantity = input.Quantity;

        await _productRepository.UpdateAsync(product, cancellationToken);
    }

    public async Task<FilteredResult<GetProductShortResult>> GetAllProducts(PaginationQueryParams queryParams, CancellationToken cancellationToken)
    {
        return await _productRepository.TableNoTracking
            .ProjectTo<GetProductShortResult>(_mapper.ConfigurationProvider)
            .ExecuteWithPaginationAsync(queryParams, cancellationToken);
    }

    public async Task<GetProductResult> GetProduct(int productId, CancellationToken cancellationToken)
    {
        var product = await _productRepository.TableNoTracking
            .Where(x => x.Id == productId)
            .ProjectTo<GetProductResult>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);

        if (product is null)
            throw new ProductNotFoundException();

        return product;
    }
}
