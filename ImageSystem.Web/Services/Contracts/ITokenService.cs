using ImageSystem.Domain;

namespace ImageSystem.Web;

public interface ITokenService
{
    public string GetToken(User account);
}
