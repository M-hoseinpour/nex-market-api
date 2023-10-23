using ApiFramework.Exceptions;
using market.Data.Repository;
using market.Exceptions;
using market.Models.Domain;
using market.Models.Enum;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;

public class PanelService
{
    private readonly IRepository<Panel> _panelRepository;
    private readonly IRepository<Manager> _managerRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Staff> _staffRepository;
    private readonly IWorkContext _workContext;

    public PanelService(
        IRepository<Panel> panelRepository,
        IRepository<Manager> managerRepository,
        IRepository<User> userRepository,
        IRepository<Staff> staffRepository,
        IWorkContext workContext
    )
    {
        _panelRepository = panelRepository;
        _managerRepository = managerRepository;
        _workContext = workContext;
        _userRepository = userRepository;
        _staffRepository = staffRepository;
    }

    public async Task AddPanel(PanelInput panelInput, CancellationToken cancellationToken)
    {
        var manager = await GetManager(cancellationToken);
        var panel = await _panelRepository.TableNoTracking.Where(p => p.ManagerId == manager.Id).SingleOrDefaultAsync(cancellationToken);
        if (panel is not null)
        {
            throw new BadRequestException("manager already has panel!");
        }

        var newPanel = new Panel { ManagerId = manager.Id, Name = panelInput.Name };
        await _panelRepository.AddAsync(newPanel, cancellationToken);
    }
    public async Task AddStaff(AddStaffInput addStaffInput, CancellationToken cancellationToken)
    {
        var manager = await GetManager(cancellationToken);

        var panel = await _panelRepository.TableNoTracking.Where(p => p.ManagerId == manager.Id).FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("manager does not have a panel");

        var user = await _userRepository.TableNoTracking
            .FirstOrDefaultAsync(u => u.Email == addStaffInput.Email && u.UserType == UserType.Staff, cancellationToken) ?? throw new NotFoundException("staff not found!");

        var staff = await _staffRepository.TableNoTracking.Where(s => s.UserId == user.Id).SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("staff not found!");

        if (staff.PanelId is not null)
        {
            throw new BadRequestException("staff is already a member of another panel");
        }

        staff.PanelId = panel.Id;
        staff.ApproverId = manager.Id;
        await _staffRepository.UpdateAsync(entity: staff, cancellationToken: cancellationToken);
    }

    public async Task<Manager> GetManager(CancellationToken cancellationToken)
    {
        var userId = _workContext.GetUserId();
        var userType = _workContext.GetUserType();
        if (UserType.Manager != userType) throw new BadIdentityException("user is not manger!");
        return await _managerRepository.TableNoTracking.Where(u => u.UserId == userId).SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("manager is not found!");
    }
}
