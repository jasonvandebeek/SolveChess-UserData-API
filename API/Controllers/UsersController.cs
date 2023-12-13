using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolveChess.API.Exceptions;
using SolveChess.API.Models;
using SolveChess.Logic.Interfaces;

namespace SolveChess.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userService.GetUser(id);
        if (user == null)
            return NotFound();

        var response = new UserDto()
        {
            Username = user.Username,
            Rating = user.Rating,
            ProfilePictureUrl = Url.Action("GetProfilePictureById", "Users", new { id }, Request.Scheme) ?? ""
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPost()]
    public async Task<IActionResult> CreateUser([FromForm] UserCreationDto model)
    {
        string userId = GetUserIdFromCookies();

        using var memoryStream = new MemoryStream();
        model.ProfilePicture?.CopyTo(memoryStream);
        var fileBytes = memoryStream.ToArray();

        await _userService.CreateUser(userId, model.Username, fileBytes);

        return Ok();
    }

    [HttpGet("{id}/username")]
    public async Task<IActionResult> GetUsernameById(string id)
    {
        var username = await _userService.GetUsername(id);
        if (username == null)
            return NotFound();

        return Ok(username);
    }

    [Authorize]
    [HttpPut("{id}/username")]
    public async Task<IActionResult> UpdateUsername(string id, [FromBody] string newUsername)
    {
        string userId = GetUserIdFromCookies();
        if (userId != id)
            return Forbid();

        await _userService.UpdateUsername(id, newUsername);
        return Ok();
    }

    [HttpGet("{id}/rating")]
    public async Task<IActionResult> GetRatingById(string id)
    {
        var rating = await _userService.GetUserRating(id);
        if (rating == null)
            return NotFound();

        return Ok(rating);
    }

    [HttpGet("{id}/profile-picture")]
    public async Task<IActionResult> GetProfilePictureById(string id)
    {
        byte[]? profilePicture = await _userService.GetProfilePicture(id);
        if (profilePicture == null || profilePicture.Length == 0)
            return NotFound();

        return File(profilePicture, "image/png", $"{id}.png");
    }

    [Authorize]
    [HttpPut("{id}/profile-picture")]
    public async Task<IActionResult> UpdateProfilePicture(string id, [FromForm] IFormFile picture)
    {
        string userId = GetUserIdFromCookies();
        if (userId != id)
            return Forbid();

        if (picture == null || picture.Length == 0)
            return NotFound();

        try
        {
            using var memoryStream = new MemoryStream();
            picture.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();

            await _userService.UpdateProfilePicture(id, fileBytes);

            return Ok("File uploaded and saved successfully");
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }

    }

    private string GetUserIdFromCookies()
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value ?? throw new InvalidJwtTokenException();
        return userId;
    }

}
