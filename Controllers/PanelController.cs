
using market.Data.Repository;
using market.Models.Domain;
using market.Models.DTO.panel;
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

    [HttpPost("users")]
    public async Task AddStaff(AddStaffInput addStaffInput, CancellationToken cancellationToken)
    {
        await _panelService.AddStaff(addStaffInput, cancellationToken);
    }

    [HttpGet("{panelGuid:Guid}")]
    public async Task<GetPanel> GetPanel(Guid panelGuid, CancellationToken cancellationToken)
    {
        return await _panelService.GetPanel(panelGuid, cancellationToken);
    }
}