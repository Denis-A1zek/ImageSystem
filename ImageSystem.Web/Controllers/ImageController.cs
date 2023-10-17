using ImageSystem.Core.Features;
using ImageSystem.Web.Common.Models;
using ImageSystem.Web.Common.Requests;
using ImageSystem.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageSystem.Web.Controllers;

[ApiController]
[Route("api/image")]
public class ImageController : BaseController
{
    private readonly IImageCreator _imageFileManager;
    private readonly IUserService _userService;

    public ImageController(IImageCreator imageFileManager, IUserService userService)
        => (_imageFileManager, _userService) = (imageFileManager, userService);

    /// <summary>
    /// Upload a picture to a remote server
    /// </summary>
    /// Sample request: POST /api/image/
    /// <param name="fileRequest">Form file</param>
    /// <returns>Image name</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid data</response>
    /// <response code="401">The user is not logged in</response>
    [HttpPost("upload")]
    [Authorize]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<string> Post([FromForm] FileReqeust fileRequest)
    {
        var imageInfo = await _imageFileManager.CreateFile(fileRequest);
        return await Mediator.Send
            (new AddUserImageCommand(imageInfo.UserId, imageInfo.FileName, imageInfo.RelativePath));
    }

    /// <summary>
    /// Get pictures of a user by id. If id is not specified, return pictures of the current user
    /// </summary>
    /// Sample request: GET /api/images/54gdg-45gg3-cgfg3-vfvd or GET /api/images (In this case, the current user)
    /// <param name="userId">User id</param>
    /// <param name="count">Number of images in the request</param>
    /// <returns>List with information about the pictures</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid data</response>
    /// <response code="401">The user is not logged in</response>
    /// <response code="404">User not found in the system</response>
    /// <response code="403">No permission to view pictures</response>
    [HttpGet("/api/images")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<ImageInfo>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetImages(Guid userId = default, int count = 5)
    {
        if(userId == Guid.Empty)
            userId = _userService.UserId;

        return Ok(await Mediator.Send(new GetUserImagesQuery(_userService.UserId, userId, count)));
    }
}
