using ImageSystem.Core.Common.Utils;
using Microsoft.Extensions.Configuration;

namespace ImageSystem.Core.Services;

public class PathManager : IPathManager
{
    private readonly IConfiguration _configuration;

    public PathManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetCurrentDirectory()
    {
        var result = Directory.GetCurrentDirectory();
        return result;
    }
    public string GetStaticContentDirectory(Guid userId)
    {
        var pathFromConfig = _configuration[ImageConstant.LocalPath];
        var result = string.IsNullOrEmpty(pathFromConfig) ?
            Path.Combine(Directory.GetCurrentDirectory(), $"Images\\{userId}")
            : $"{pathFromConfig}\\{userId}";

        if (!Directory.Exists(result))
        {
            Directory.CreateDirectory(result);
        }
        return result;
    }
    public string GetUserPersonalFolder(Guid userId, string fileName)
    {
        var staticContentDirectory = GetStaticContentDirectory(userId);
        var result = Path.Combine(staticContentDirectory, fileName);
        return result;
    }
}
