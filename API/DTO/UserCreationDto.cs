using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.Models;

public class UserCreationDto
{

    public string? Username { get; set; }

    public IFormFile? ProfilePicture { get; set; }

}
