using ImageSystem.Web.Common;
using ImageSystem.Web.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageSystem.Web.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
        => _userService = userService;

    /// <summary>
    /// Log in
    /// </summary>
    /// <remarks>
    /// Sample request: POST /api/login/user
    /// {
    ///     username: someName123
    ///     password: password123
    /// }
    /// </remarks>
    /// <param name="request">User data</param>
    /// <returns>Token</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Invalid data</response>
    /// <response code="404">User not found in the system</response>

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginRequest request)
        => Ok(await _userService.LoginAsync(request));

    /// <summary>
    /// Registration
    /// </summary>
    /// Sample request: POST /api/user/register
    /// {
    ///     username: someName123
    ///     password: password123
    /// }
    /// <param name="request">User data</param>
    /// <returns>User id</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    /// <response code="200">Success</response>
    /// <response code="400">Invalid data or user exists</response>
    public async Task<IActionResult> Register(RegisterRequest request)
        => Ok(await _userService.RegisterAsync(request));
}
