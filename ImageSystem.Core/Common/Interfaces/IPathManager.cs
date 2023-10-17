namespace ImageSystem.Core;

public interface IPathManager
{
    string GetCurrentDirectory();
    string GetStaticContentDirectory(Guid userId);
    string GetUserPersonalFolder(Guid userId, string fileName);
}
