using ImageSystem.Web.Common.Models;
using ImageSystem.Web.Common.Requests;

namespace ImageSystem.Web.Services;

public interface IImageCreator
{
    Task<CreatedImageInfo> CreateFile(FileReqeust fileRequest);
}
