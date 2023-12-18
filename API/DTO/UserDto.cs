using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.Models;

public class UserDto
{

    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    public int Rating { get; set; }

    [Required]
    public string ProfilePictureUrl { get; set; } = null!;

}