using ImageSystem.Core;
using ImageSystem.Web.Common.Exceptions;
using ImageSystem.Web.Common.Models;
using ImageSystem.Web.Common.Requests;

namespace ImageSystem.Web.Services;

public class ImageCreator : IImageCreator
{
    private readonly IUserService _userService;
    private readonly IPathManager _pathManager;

    private const int MAX_IMAGE_SIZE = 200 * 1024 * 1024;

    public ImageCreator(
        IUserService userService,
        IPathManager pathManager)
    {
        _userService = userService;
        _pathManager = pathManager;
    }

    public async Task<CreatedImageInfo> CreateFile(FileReqeust fileRequest)
    {
        if (fileRequest.File.Length <= 0)
            throw new ImageLoadingException("Файл поврежден или битый");

        if (fileRequest.File.Length > MAX_IMAGE_SIZE)
            throw new ImageLoadingException("Размер файла превышает 200 Мб");

        var userId = _userService.UserId;
        
        FileInfo fileInfo = new FileInfo(fileRequest.File.FileName);
        var fileName = GenerateImageName(fileInfo);
        var absolutePath = _pathManager.GetUserPersonalFolder(userId, fileName);
        using (var fileStream = new FileStream(absolutePath, FileMode.Create))
        {
            await fileRequest.File.CopyToAsync(fileStream);
        }
        
        var urnPath = $"{userId}/{fileName}";
        return new CreatedImageInfo(userId, fileName, urnPath);
    }

    private string GenerateImageName(FileInfo fileInfo)
    {
        string fileNameWithoutExtension = Path.ChangeExtension(fileInfo.Name, null);
        return fileNameWithoutExtension + "_" + DateTime.Now.Ticks.ToString() + fileInfo.Extension;
    }
}
