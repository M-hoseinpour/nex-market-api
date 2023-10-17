using ApiFramework.Exceptions;
using market.Data.Repository;
using market.Exceptions;
using market.Models.Domain;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;

public class PanelService
{
    private readonly IRepository<Panel> _panelRepository;
    private readonly IRepository<Manager> _managerRepository;
    private readonly IWorkContext _workContext;

    public PanelService(
        IRepository<Panel> panelRepository,
        IRepository<Manager> managerRepository,
        IWorkContext workContext
    )
    {
        _panelRepository = panelRepository;
        _managerRepository = managerRepository;
        _workContext = workContext;
    }

    public async Task AddPanel(PanelInput panelInput, CancellationToken cancellationToken)
    {
        var userId = _workContext.GetUserId();
        var roleId = _workContext.GetRoleId();

        if (Roles.Manager.Id != roleId) throw new BadIdentityException("user is not manger!");

        var manager = await _managerRepository.TableNoTracking.Where(u => u.UserId == userId).SingleOrDefaultAsync() ?? throw new NotFoundException("user is not found!");
        var panel = new Panel { ManagerId = manager.Id, Name = panelInput.Name };
        await _panelRepository.AddAsync(panel, cancellationToken);
    }
}
