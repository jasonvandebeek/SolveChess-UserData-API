using SolveChess.Logic.DTO;

namespace SolveChess.Logic.DAL;

public interface IUserDataDAL
{

    public string? GetUsername(string userID);
    public void UpdateUsername(string userID, string newUsername);
    public int? GetUserRating(string userID);
    public UserDTO? GetUser(string userID);
    public byte[]? GetProfilePicture(string userID);
    public void UpdateProfilePicture(string userID, byte[] picture);

}