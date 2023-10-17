
using market.Data.Repository;
using market.Models.Domain;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/panels")]
public class PanelsController : ControllerBase
{
    private readonly PanelService _panelService;

    public PanelsController(PanelService panelService, IRepository<Manager> mangerRepository)
    {
        _panelService = panelService;
    }

    [HttpPost]
    public async Task AddPanel(PanelInput panelInput, CancellationToken cancellationToken)
    {
        await _panelService.AddPanel(panelInput, cancellationToken);
    }
}