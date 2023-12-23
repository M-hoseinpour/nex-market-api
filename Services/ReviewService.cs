using AutoMapper;
using AutoMapper.QueryableExtensions;
using market.Data.Repository;
using market.Exceptions;
using market.Extensions;
using market.Models.Domain;
using market.Models.DTO.BaseDto;
using market.Models.DTO.Review;
using market.Services.ProductService.Exceptions;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;

namespace market.Services;

public class ReviewService
{
    private readonly IWorkContext _workContext;
    private readonly IRepository<Review> _reviewRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public ReviewService(
        IWorkContext workContext,
        IRepository<Review> reviewRepository,
        IRepository<Product> productRepository,
        IMapper mapper
    )
    {
        _workContext = workContext;
        _reviewRepository = reviewRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task AddReview(
        Guid productGuid,
        ReviewInput input,
        CancellationToken cancellationToken
    )
    {
        var customerId = _workContext.GetCustomerId();

        if (input.Rating is > 5 or < 0)
            throw new BadRequestException();

        var product = await _productRepository.TableNoTracking.SingleOrDefaultAsync(
            x => x.Uuid == productGuid,
            cancellationToken
        );

        if (product is null)
            throw new ProductNotFoundException();

        var review = new Review
        {
            CustomerId = customerId,
            ProductId = product.Id,
            Rating = input.Rating,
            Comment = input.Comment
        };

        await _reviewRepository.AddAsync(review, cancellationToken);

        await UpdateProductRating(productId: product.Id, cancellationToken: cancellationToken);
    }

    private async Task UpdateProductRating(int productId, CancellationToken cancellationToken)
    {
        var product = await _productRepository.Table
            .Where(x => x.Id == productId)
            .Include(x => x.Reviews)
            .SingleOrDefaultAsync(cancellationToken);

        if (product?.Reviews is null)
            throw new ProductNotFoundException();

        var newRating = product.Reviews.Average(x => x.Rating);

        product.Rating = newRating;

        await _productRepository.UpdateAsync(product, cancellationToken);
    }

    public async Task<FilteredResult<ReviewResult>> GetReviews(
        Guid productGuid,
        PaginationQueryParams queryParams,
        CancellationToken cancellationToken
    )
    {
        return await _reviewRepository.TableNoTracking
            .Where(x => x.Product.Uuid == productGuid)
            .OrderByDescending(x => x.CreateMoment)
            .ProjectTo<ReviewResult>(_mapper.ConfigurationProvider)
            .ExecuteWithPaginationAsync(queryParams, cancellationToken);
    }
}
