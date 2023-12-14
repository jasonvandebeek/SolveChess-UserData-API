using System.ComponentModel.DataAnnotations;

namespace SolveChess.Logic.Models;

public class User
{

    [Required]
    public string Id { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public int Rating { get; set; }

    public byte[]? ProfilePicture { get; set; }

}