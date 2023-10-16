using market.Data.Repository;
using market.Models.Domain;
using market.Models.DTO.User;
using Microsoft.EntityFrameworkCore;
using market.SystemServices.Contracts;
using System.Security.Claims;
using market.Exceptions;
using market.Services.UserService.Exceptions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using market.Models.Enum;
using Microsoft.IdentityModel.Tokens;
using market.Data.Domain;
using market.Migrations;

public class UserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Staff> _staffRepository;
    private readonly IRepository<Manager> _managerRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly IWorkContext _workContext;
    private readonly IMapper _mapper;


    public UserService(
        IRepository<User> userRepository,
        IRepository<Customer> customerRepository,
        IRepository<Staff> staffRepository,
        IRepository<Manager> managerRepository,
        IPasswordService passwordService,
        IJwtService jwtService,
        IWorkContext workContext,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _staffRepository = staffRepository;
        _managerRepository = managerRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _workContext = workContext;
        _mapper = mapper;
    }

    public async Task<RegisterResponse> RegisterUser(RegisterInput input, CancellationToken cancellationToken)
    {

        var userRole = Roles.Customer;
        if (input.UserType == UserType.Staff)
        {
            userRole = Roles.Staff;
        }
        if (input.UserType == UserType.Manager)
        {
            userRole = Roles.Manager;
        }

        var user = await _userRepository.TableNoTracking
          .SingleOrDefaultAsync(x => x.Email == input.Email && x.Role == userRole, cancellationToken);

        if (user is not null)
            throw new AlreadyRegisteredException();

        var newUser = new User
        {
            Password = _passwordService.Hash(input.Password),
            Email = input.Email,
            RoleId = userRole.Id
        };

        await _userRepository.AddAsync(newUser, cancellationToken);

        if (input.UserType == UserType.Customer)
        {
            var newCustomer = new Customer { User = newUser, Addresses = null };
            await _customerRepository.AddAsync(newCustomer, cancellationToken);
        }

        if (input.UserType == UserType.Staff)
        {
            var newStaff = new Staff { User = newUser };
            await _staffRepository.AddAsync(newStaff, cancellationToken);
        }

        if (input.UserType == UserType.Manager)
        {
            var newManager = new Manager { User = newUser };
            await _managerRepository.AddAsync(newManager, cancellationToken);
        }

        var claims = new List<Claim>
        {
            new(type: "user-id", value: newUser.Id.ToString()),
            new(type: "role-id", value: newUser.RoleId.ToString())
        };

        long expiresIn = 0;
        var token = _jwtService.GenerateToken(claims, ref expiresIn);

        return new RegisterResponse
        {
            Token = token
        };
    }

    public async Task<RegisterResponse> LoginUser(RegisterInput input, CancellationToken cancellationToken)
    {
        var role = Roles.Customer;
        if (input.UserType == UserType.Staff)
        {
            role = Roles.Staff;
        }
        if (input.UserType == UserType.Manager)
        {
            role = Roles.Manager;
        }

        var user = await _userRepository.Table
          .Where(x => x.Email == input.Email && x.Role == role)
          .SingleOrDefaultAsync(cancellationToken) ?? throw new UserNotFoundException();

        if (_passwordService.Verify(input.Password, user.Password) is false)
            throw new BadCredentialsException();

        var claims = new List<Claim>
        {
            new(type: "user-id", value: user.Id.ToString()),
            new(type: "role-id", value: user.RoleId.ToString())
        };

        long expiresIn = 0;
        var token = _jwtService.GenerateToken(claims, ref expiresIn);

        return new RegisterResponse { Token = token };

    }

    public async Task<ProfileBriefResponse> GetBriefProfile(CancellationToken cancellationToken)
    {
        var userId = _workContext.GetUserId();
        var roleId = _workContext.GetRoleId();

        return await _userRepository.Table
            .Where(x => x.Id == userId && x.RoleId == roleId)
            .ProjectTo<ProfileBriefResponse>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken) ?? throw new UserNotFoundException();
    }

    public async Task UpdateProfileField(
        string field,
        UpdateFieldInput input,
        CancellationToken cancellationToken
        )
    {
        var userId = _workContext.GetUserId();
        var roleId = _workContext.GetRoleId();

        var user = await _userRepository.Table
            .Where(x => x.Id == userId && x.RoleId == roleId)
            .SingleOrDefaultAsync(cancellationToken) ?? throw new UserNotFoundException();

        if (!Enum.TryParse<UserUpdateField>(
                    value: field,
                    ignoreCase: true,
                    result: out var updateField
                )
            )
        {
            throw new BadRequestException("Field not Found!");
        }

        switch (updateField)
        {
            case UserUpdateField.FirstName:
                user.FirstName = input.FieldValue;
                break;
            case UserUpdateField.LastName:
                user.LastName = input.FieldValue;
                break;
            case UserUpdateField.MobileNumber:
                user.MobileNumber = input.FieldValue;
                break;
            case UserUpdateField.AvatarLogo:
                user.AvatarLogo = input.FieldValue;
                break;
            case UserUpdateField.Setting:
                user.Setting = input.FieldValue;
                break;
            default:
                throw new BadRequestException();
        }

        await _userRepository.UpdateAsync(entity: user, cancellationToken: cancellationToken);
    }

    public async Task<ProfileBriefResponse> CompleteProfile(ProfileInput input, CancellationToken cancellationToken)
    {
        var userId = _workContext.GetUserId();
        var roleId = _workContext.GetRoleId();

        var currentUser = await _userRepository.Table
            .Where(x => x.Id == userId && roleId == x.RoleId)
            .SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("user not Found!");

        currentUser.FirstName = input.FirstName;
        currentUser.LastName = input.LastName;
        currentUser.MobileNumber = input.MobileNumber;
        currentUser.AvatarLogo = input.AvatarLogo;

        if (input.FirstName.IsNullOrEmpty() || input.LastName.IsNullOrEmpty() || input.MobileNumber.IsNullOrEmpty())
            throw new BadRequestException();

        await _userRepository.UpdateAsync(currentUser, cancellationToken);

        return _mapper.Map<ProfileBriefResponse>(currentUser);
    }
}