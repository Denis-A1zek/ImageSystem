using ImageSystem.Core.Features;
using ImageSystem.Web.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageSystem.Web.Controllers;

[ApiController]
[Route("api/friendship")]
public class FriendshipController : BaseController
{
    private  readonly IUserService _userService;

    public FriendshipController(IUserService userService)
        => _userService = userService;

    /// <summary>
    /// Send a friend request
    /// </summary>
    /// Sample request: POST /api/friendship/add
    /// <param name="Reciver">Reciever id</param>
    /// <returns>Reciever id</returns>
    /// <response code="200">Success</response>
    /// <response code="401">The user is not logged in</response>
    /// <response code="400">Invalid data or retry sending</response>
    /// <response code="404">User not found in the system</response>
    [HttpPost("add")]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PostFriendRequest(Guid reciver)
    {
        var currentUserId = _userService.UserId;
        return Ok(await Mediator.Send(new CreateFriendshipReqeustCommand(currentUserId, reciver)));
    }
}
