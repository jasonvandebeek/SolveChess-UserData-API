using System.ComponentModel.DataAnnotations;

namespace SolveChess.Logic.Models;

public class User
{

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public int Rating { get; set; }

}