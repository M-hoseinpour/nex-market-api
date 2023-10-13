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

        var newUser = new User { Password = _passwordService.Hash(input.Password), Email = input.Email };

        await _userRepository.AddAsync(newUser, cancellationToken);

        var claims = new List<Claim>
        {
            new(type: "user-id", value: newUser.Id.ToString())
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

        var claims = new List<Claim> { new(type: "user-id", value: user.Id.ToString()) };

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
}