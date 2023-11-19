
using SolveChess.Logic.DTO;

namespace SolveChess.Logic.ServiceInterfaces;

public interface IUserService
{

    public string? GetUsername(string userId);
    public void UpdateUsername(string userId, string username);
    public int? GetUserRating(string userId);
    public UserDTO? GetUser(string userId);
    public byte[]? GetProfilePicture(string userId);
    public void UpdateProfilePicture(string userId, byte[] picture);
    public void CreateUser(string userId, string username, byte[]? picture);

}
