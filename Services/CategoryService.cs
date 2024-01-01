using AutoMapper;
using AutoMapper.QueryableExtensions;
using market.Data.Repository;
using market.Exceptions;
using market.Extensions;
using market.Models.Domain;
using market.Models.DTO.BaseDto;
using market.Models.DTO.Brand;
using market.Models.DTO.Category;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;

namespace market.Services;

public class CategoryService
{
    private readonly IWorkContext _workContext;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(
        IWorkContext workContext,
        IRepository<Category> categoryRepository,
        IMapper mapper
    )
    {
        _workContext = workContext;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task AddCategory(CategoryInput input, CancellationToken cancellationToken)
    {
        var panelId = _workContext.GetPanelId();

        var category = new Category { Name = input.Name, PanelId = panelId, };

        if (input.ParentUuid.HasValue)
        {
            var parentCategory = await _categoryRepository.TableNoTracking.SingleOrDefaultAsync(
                x => x.Uuid == input.ParentUuid,
                cancellationToken
            );

            if (parentCategory is null)
                throw new NotFoundException();

            category.ParentCategoryId = parentCategory.Id;
        }

        await _categoryRepository.AddAsync(category, cancellationToken);
    }

    public async Task<FilteredResult<CategoryResult>> GetCategories(
        GetCategoriesQueryParams queryParams,
        CancellationToken cancellationToken,
        bool isAllCategories = false
    )
    {

        var categoryQuery = _categoryRepository.TableNoTracking;

        if (isAllCategories)
        {
            var panelId = _workContext.GetPanelId();    
            categoryQuery = categoryQuery.Where(x => x.PanelId == panelId);   
        }

        if (queryParams.OnlyParentCategories.HasValue)
            categoryQuery = categoryQuery.Where(x => x.ParentCategoryId == null);

        if (queryParams.ParentCategoryUuid.HasValue)
        {
            var parentCategory = await _categoryRepository.TableNoTracking.SingleOrDefaultAsync(
                x => x.Uuid == queryParams.ParentCategoryUuid,
                cancellationToken
            );

            if (parentCategory is null)
                throw new NotFoundException();

            categoryQuery = categoryQuery.Where(x => x.ParentCategoryId == parentCategory.Id);
        }

        var categories = await categoryQuery
            .ProjectTo<CategoryResult>(_mapper.ConfigurationProvider)
            .ExecuteWithPaginationAsync(queryParams, cancellationToken);

        return categories;
    }
}
