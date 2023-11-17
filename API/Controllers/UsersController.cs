using Microsoft.AspNetCore.Mvc;
using SolveChess.API.Models;
using SolveChess.Logic.ServiceInterfaces;

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
    public IActionResult GetUserById(string id)
    {
        var user = _userService.GetUser(id);
        if (user == null)
            return NotFound();

        var response = new UserModel()
        {
            Username = user.Username,
            Rating = user.Rating,
            ProfilePictureUrl = Url.Action("GetProfilePictureById", "Users", new { id }, Request.Scheme) ?? ""
    };

        return Ok(response);
    }

    [HttpGet("{id}/username")]
    public IActionResult GetUsernameById(string id)
    {
        var username = _userService.GetUsername(id);
        if (username == null)
            return NotFound();

        return Ok(username);
    }

    [HttpPut("{id}/username")]
    public IActionResult UpdateUsername(string id, [FromBody] string newUsername)
    {
        _userService.UpdateUsername(id, newUsername);
        return Ok();
    }

    [HttpGet("{id}/rating")]
    public IActionResult GetRatingById(string id)
    {
        var rating = _userService.GetUserRating(id);
        if (rating == null)
            return NotFound();

        return Ok(rating);
    }

    [HttpGet("{id}/profile-picture")]
    public IActionResult GetProfilePictureById(string id)
    {
        byte[]? profilePicture = _userService.GetProfilePicture(id);
        if (profilePicture == null || profilePicture.Length == 0)
            return NotFound();

        return File(profilePicture, "image/png", $"{id}.png");
    }

    [HttpPut("{id}/profile-picture")]
    public IActionResult UpdateProfilePicture(string id, [FromForm] IFormFile picture)
    {
        if (picture == null || picture.Length == 0)
            return NotFound();

        try
        {
            using (var memoryStream = new MemoryStream())
            {
                picture.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();

                _userService.UpdateProfilePicture(id, fileBytes);
            }

            return Ok("File uploaded and saved successfully");
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }

    }

}
