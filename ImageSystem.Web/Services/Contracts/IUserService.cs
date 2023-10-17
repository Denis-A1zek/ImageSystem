using ImageSystem.Web.Common;

namespace ImageSystem.Web;

public interface IUserService
{
    Guid UserId { get; }

    Task<string> LoginAsync(LoginRequest request);
    Task<Guid> RegisterAsync(RegisterRequest request);
}

