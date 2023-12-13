using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolveChess.DAL.Model;

public class UserModel
{

    [Key]
    public string Id { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    [DefaultValue(100)]
    public int Rating { get; set; }

    [Column(TypeName = "LONGBLOB")]
    public byte[]? ProfilePicture { get; set; }

}