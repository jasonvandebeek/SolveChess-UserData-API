using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.Models;

public class UserCreationModel
{

    [Required]
    public string Username { get; set; }
    public IFormFile? ProfilePicture { get; set; }

}
