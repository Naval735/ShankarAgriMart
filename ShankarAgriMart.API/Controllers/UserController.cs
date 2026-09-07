using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShankarAgriMart.Application.DTOs.Request;
using ShankarAgriMart.Application.Interfaces.Services;

namespace ShankarAgriMart.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/User/profile
    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var userId = GetUserId();

        var profile = await _userService
            .GetProfileAsync(userId);

        return Ok(new
        {
            Success = true,
            Data = profile
        });
    }

    // PUT: api/User/profile
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateUserProfileRequest request)
    {
        var userId = GetUserId();

        var profile = await _userService
            .UpdateProfileAsync(userId, request);

        return Ok(new
        {
            Success = true,
            Message = "Profile updated successfully.",
            Data = profile
        });
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException(
                "User ID was not found in the token.");

        return userId;
    }
}