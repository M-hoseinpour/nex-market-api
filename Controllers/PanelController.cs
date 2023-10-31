using market.Data.Repository;
using market.Models.Domain;
using market.Models.DTO.panel;
using market.Models.DTO.Panel;
using Microsoft.AspNetCore.Mvc;

namespace market.Controllers;

[ApiController]
[Route("api/panels")]
public class PanelController : ControllerBase
{
    private readonly PanelService _panelService;

    public PanelController(PanelService panelService, IRepository<Manager> mangerRepository)
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

    [HttpGet("{panelGuid:Guid}/members")]
    public async Task<IList<PanelMember>?> GetPanelMembers(Guid panelGuid, CancellationToken cancellationToken)
    {
        return await _panelService.GetPanelMembers(panelGuid, cancellationToken);
    }
}