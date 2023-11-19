using SolveChess.Logic.DTO;

namespace SolveChess.Logic.DAL;

public interface IUserDataDAL
{

    public string? GetUsername(string userId);
    public void UpdateUsername(string userId, string newUsername);
    public int? GetUserRating(string userId);
    public UserDTO? GetUser(string userId);
    public byte[]? GetProfilePicture(string userId);
    public void UpdateProfilePicture(string userId, byte[] picture);
    public void CreateUser(string userId, string username, byte[]? profilePicture);

}