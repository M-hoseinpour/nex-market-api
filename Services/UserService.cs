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

public class UserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly IWorkContext _workContext;
    private readonly IMapper _mapper;


    public UserService(IRepository<User> userRepository, IPasswordService passwordService, IJwtService jwtService, IWorkContext workContext, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _workContext = workContext;
        _mapper = mapper;
    }

    public async Task<RegisterResponse> RegisterUser(RegisterInput input, CancellationToken cancellationToken)
    {
        var user = await _userRepository.TableNoTracking
        .SingleOrDefaultAsync(x => x.Email == input.Email);

        if (user is not null)
            throw new AlreadyRegisteredException();

        var newUser = new User { Password = _passwordService.Hash(input.Password), Email = input.Email, RoleId = Roles.Customer.Id };

        await _userRepository.AddAsync(newUser, cancellationToken);

        var claims = new List<Claim>
        {
            new(type: "user-id", value: newUser.Id.ToString()),
            new(type: "role-id", value: newUser.RoleId.ToString())
        };

        long expiresIn = 0;
        var token = _jwtService.GenerateToken(claims, ref expiresIn);

        return new RegisterResponse { Token = token };
    }

    public async Task<RegisterResponse> LoginUser(RegisterInput input, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Table
          .Where(x => x.Email == input.Email)
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
        return await _userRepository.Table
            .Where(x => x.Id == userId)
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
        var user = await _userRepository.GetByIdAsync(cancellationToken: cancellationToken, userId) ?? throw new UserNotFoundException();

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

        var currentUser = await _userRepository.Table
            .Where(x => x.Id == userId)
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