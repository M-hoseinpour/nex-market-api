using ApiFramework.Exceptions;
using AutoMapper;
using market.Data.Repository;
using market.Exceptions;
using market.Models.Domain;
using market.Models.DTO.panel;
using market.Models.DTO.Panel;
using market.Models.Enum;
using market.SystemServices.Contracts;
using Microsoft.EntityFrameworkCore;

public class PanelService
{
    private readonly IRepository<Panel> _panelRepository;
    private readonly IRepository<Manager> _managerRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Staff> _staffRepository;
    private readonly UserService _userService;
    private readonly IWorkContext _workContext;
    private readonly IMapper _mapper;

    public PanelService(
        IRepository<Panel> panelRepository,
        IRepository<Manager> managerRepository,
        IRepository<User> userRepository,
        IRepository<Staff> staffRepository,
        IWorkContext workContext,
        IMapper mapper,
        UserService userService
    )
    {
        _panelRepository = panelRepository;
        _managerRepository = managerRepository;
        _workContext = workContext;
        _userRepository = userRepository;
        _staffRepository = staffRepository;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task AddPanel(PanelInput panelInput, CancellationToken cancellationToken)
    {
        var manager = await GetManager(cancellationToken);
        var panel = await _panelRepository.TableNoTracking
            .Where(p => p.ManagerId == manager.Id)
            .SingleOrDefaultAsync(cancellationToken);
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

        var panel =
            await _panelRepository.TableNoTracking
                .Where(p => p.ManagerId == manager.Id)
                .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("manager does not have a panel");

        var user =
            await _userRepository.TableNoTracking.FirstOrDefaultAsync(
                u => u.Email == addStaffInput.Email && u.UserType == UserType.Staff,
                cancellationToken
            ) ?? throw new NotFoundException("staff not found!");

        var staff =
            await _staffRepository.TableNoTracking
                .Where(s => s.UserId == user.Id)
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("staff not found!");

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
        if (UserType.Manager != userType)
            throw new BadIdentityException("user is not manger!");
        return await _managerRepository.TableNoTracking
                .Where(u => u.UserId == userId)
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("manager is not found!");
    }

    public async Task<GetPanel> GetPanel(CancellationToken cancellationToken)
    {
        var panelId = await GetPanelId(cancellationToken);

        var panel =
            await _panelRepository.TableNoTracking
                .Where(p => p.Id == panelId)
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("panel not found!");

        return _mapper.Map<GetPanel>(panel);
    }

    public async Task<Panel?> GetUserPanel()
    {
        var userId = _workContext.GetUserId();
        var userType = _workContext.GetUserType();

        if (UserType.Staff == userType)
        {
            var staffId = _workContext.GetStaffId();
            var staff =
                await _staffRepository.TableNoTracking
                    .Where(S => S.Id == staffId)
                    .Include(m => m.Panel)
                    .SingleOrDefaultAsync() ?? throw new UserNotFoundException();
            if (staff.Panel is not null)
                return staff.Panel;
        }

        if (UserType.Manager == userType)
        {
            var managerId = _workContext.GetManagerId();
            var manager =
                await _managerRepository.TableNoTracking
                    .Where(S => S.Id == managerId)
                    .Include(m => m.Panel)
                    .SingleOrDefaultAsync() ?? throw new UserNotFoundException();
            if (manager.Panel is not null)
                return manager.Panel;
        }
        return null;
    }

    public async Task<IList<PanelMember>?> GetPanelMembers(
        Guid panelGuid,
        CancellationToken cancellationToken
    )
    {
        var userPanel =
            await GetUserPanel() ?? throw new NotFoundException("user does Not have a panel!");
        var panel =
            await _panelRepository.TableNoTracking
                .Where(p => p.Uuid == panelGuid)
                .Include(p => p.Staffs)
                .ThenInclude(s => s.User)
                .Include(p => p.Manager)
                .ThenInclude(m => m.User)
                .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("panel not found!");

        if (userPanel.Id != panel.Id)
        {
            throw new NotFoundException("panel not found!");
        }

        var staffMembers = _mapper.Map<IList<PanelMember>>(panel.Staffs);
        var managerMember = _mapper.Map<PanelMember>(panel.Manager.User);

        staffMembers.Insert(0, managerMember);

        return staffMembers;
    }

    public async Task<int> GetPanelId(CancellationToken cancellationToken)
    {
        var panelId = _workContext.GetPanelId();

        if (panelId is null)
        {
            var managerId = _workContext.GetManagerId();
            var panel = await _panelRepository.TableNoTracking.FirstOrDefaultAsync(p => p.ManagerId == managerId, cancellationToken) ?? throw new NotFoundException("panel not found");
            panelId = panel.Id;
        }

        return panelId ?? -1;
    }
}
