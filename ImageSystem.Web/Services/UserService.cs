using ImageSystem.Core;
using ImageSystem.Domain;
using ImageSystem.Infrastructure;
using ImageSystem.Web.Common;
using System.Security.Claims;

namespace ImageSystem.Web.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public UserService
        (IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        ILogger<UserService> logger,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public Guid UserId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.SerialNumber);
            return string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
        }
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation($"Request to login from a user {request.Username}");
        var userRepository = _unitOfWork.GetRepository<User>();
        var user = await userRepository
            .FindAsync(a => a.Name.ToLower().Equals(request.Username.ToLower()));
        var userAccount = user.FirstOrDefault();
        if (userAccount == null)
            throw new NotFoundException("Account", request.Username);

        var passwordValid = _passwordHasher.Verify(userAccount.Password, request.Password);
        if (!passwordValid) 
            throw new IncorrectPasswordException($"{request.Username}");

        var token = _tokenService.GetToken(userAccount);

        return token;
    }

    public async Task<Guid> RegisterAsync(RegisterRequest request)
    {
        _logger.LogInformation($"Request to register from a user {request.Username}");
        var userRepository = _unitOfWork.GetRepository<User>();
        var exsistingUser = await userRepository
            .FindAsync(a => a.Name.ToLower().Equals(request.Username.ToLower()));

        if (exsistingUser is not null)
            throw new ArgumentException($"Пользователь c ником {request.Username} уже существует");

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = request.Username,
            Password = passwordHash
        };

        await userRepository.InsertAsync(user);
        await _unitOfWork.SaveChangesAsync();    
        return user.Id;
    }
}
