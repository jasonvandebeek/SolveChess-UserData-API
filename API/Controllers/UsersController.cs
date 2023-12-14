﻿using Microsoft.AspNetCore.Authorization;
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

        var baseUrl = $"{Request.Scheme}";

        var forwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        var forwardedProto = Request.Headers["X-Forwarded-Proto"].FirstOrDefault();

        if (!string.IsNullOrEmpty(forwardedFor))
        {
            baseUrl = $"{forwardedProto}://{forwardedFor}";
        }

        var response = new UserDto()
        {
            Username = user.Username,
            Rating = user.Rating,
            ProfilePictureUrl = Url.Action("GetProfilePictureById", "Users", new { id }, baseUrl) ?? ""
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

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("{id}/username")]
    public async Task<IActionResult> GetUsernameById(string id)
    {
        var user = await _userService.GetUser(id);
        if (user == null)
            return NotFound();

        return Ok(user.Username);
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
        var user = await _userService.GetUser(id);
        if (user == null)
            return NotFound();

        return Ok(user.Rating);
    }

    [HttpGet("{id}/profile-picture")]
    public async Task<IActionResult> GetProfilePictureById(string id)
    {
        var user = await _userService.GetUser(id);
        if (user == null)
            return NotFound();

        if (user.ProfilePicture == null || user.ProfilePicture.Length == 0)
            return Ok(null);

        return File(user.ProfilePicture, "image/png", $"{id}.png");
    }

    [Authorize]
    [HttpPut("{id}/profile-picture")]
    public async Task<IActionResult> UpdateProfilePicture(string id, [FromForm] IFormFile picture)
    {
        string userId = GetUserIdFromCookies();
        if (userId != id)
            return Forbid();

        if (picture == null || picture.Length == 0)
            return BadRequest();

        using var memoryStream = new MemoryStream();
        picture.CopyTo(memoryStream);
        var fileBytes = memoryStream.ToArray();

        await _userService.UpdateProfilePicture(id, fileBytes);

        return Ok("File uploaded and saved successfully");
    }

    private string GetUserIdFromCookies()
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value ?? throw new InvalidJwtTokenException();
        return userId;
    }

}
