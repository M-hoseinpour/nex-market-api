using AutoMapper;
using AutoMapper.QueryableExtensions;
using market.Data.Repository;
using market.Exceptions;
using market.Extensions;
using market.Models.Domain;
using market.Models.DTO.BaseDto;
using market.Models.DTO.Brand;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;

namespace market.Services.BrandService;

public class BrandService
{
    private readonly IWorkContext _workContext;
    private readonly IMapper _mapper;
    private readonly IRepository<Brand> _brandRepository;
    private readonly FileService.FileService _fileService;

    public BrandService(
        IRepository<Brand> brandRepository,
        IWorkContext workContext,
        IMapper mapper, FileService.FileService fileService)
    {
        _brandRepository = brandRepository;
        _workContext = workContext;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task AddBrand(BrandInput input, CancellationToken cancellationToken)
    {
        var panelId = _workContext.GetPanelId();

        var brand = new Brand { Name = input.Name, PanelId = panelId, };

        if (input.LogoFileId.HasValue)
            brand.LogoFileId = input.LogoFileId;

        await _brandRepository.AddAsync(brand, cancellationToken);
    }

    public async Task<FilteredResult<BrandResult>> GetBrands(
        PaginationQueryParams queryParams,
        CancellationToken cancellationToken,
        bool isAllBrands = false
    )
    {
        var brandQuery = _brandRepository.TableNoTracking;

        if (isAllBrands)
        {
            var panelId = _workContext.GetPanelId();
            brandQuery = brandQuery.Where(x => x.PanelId == panelId);
        }

        var brands = await brandQuery
            .ProjectTo<BrandResult>(_mapper.ConfigurationProvider)
            .ExecuteWithPaginationAsync(queryParams, cancellationToken);

        foreach (var brand in brands.Data)
        {
            if (brand.LogoFile is null) continue;

            var url = await _fileService.GetFileUrl(brand.LogoFile.FileId, cancellationToken);

            brand.LogoFile.Url = url.Url;
        }

        return brands;
    }

    public async Task DeleteBrand(Guid brandGuid, CancellationToken cancellationToken)
    {
        var panelId = _workContext.GetPanelId();

        var brand = await _brandRepository.Table
            .SingleOrDefaultAsync(x => x.Uuid == brandGuid && x.PanelId == panelId, cancellationToken);

        if (brand is null)
            throw new NotFoundException();

        await _brandRepository.DeleteAsync(brand, cancellationToken);
    }
}