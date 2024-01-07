using AutoMapper;
using AutoMapper.QueryableExtensions;
using market.Data.Repository;
using market.Extensions;
using market.Models.Domain;
using market.Models.DTO.Banner;
using market.Models.DTO.BaseDto;
using market.SystemServices.Contracts;

namespace market.Services;

public class BannerService
{
    private readonly IRepository<Banner> _bannerRepository;
    private readonly IWorkContext _workContext;
    private readonly PanelService _panelService;
    private readonly IMapper _mapper;

    public BannerService(IRepository<Banner> bannerRepository, IWorkContext workContext, IMapper mapper, PanelService panelService)
    {
        _bannerRepository = bannerRepository;
        _workContext = workContext;
        _mapper = mapper;
        _panelService = panelService;
    }

    public async Task AddBanner(BannerInput input, CancellationToken cancellationToken)
    {
        var panelId = await _panelService.GetPanelId(cancellationToken);

        var newBanner = new Banner
        {
            Title = input.Title,
            FileId = input.FileId,
            PanelId = panelId
        };

        await _bannerRepository.AddAsync(newBanner, cancellationToken);
    }

    public async Task<FilteredResult<BannerResult>> GetBanners(
        PaginationQueryParams queryParams,
        CancellationToken cancellationToken,
        bool isAllBanners = false
    )
    {
        var bannerQuery = _bannerRepository.TableNoTracking;

        if (isAllBanners)
        {
            var panelId = await _panelService.GetPanelId(cancellationToken);
            bannerQuery = bannerQuery.Where(x => x.PanelId == panelId);
        }

        return await bannerQuery
            .OrderByDescending(x => x.CreateMoment)
            .ProjectTo<BannerResult>(_mapper.ConfigurationProvider)
            .ExecuteWithPaginationAsync(queryParams, cancellationToken);
    }
}
