using SolveChess.Logic.DAL;
using SolveChess.Logic.DTO;
using SolveChess.Logic.ServiceInterfaces;

namespace SolveChess.Logic.Service;

public class UserService : IUserService
{

    private readonly IUserDataDAL _userDataDAL;

    public UserService(IUserDataDAL userDataDAL)
    {
        _userDataDAL = userDataDAL;
    }

    public string? GetUsername(string userID)
    {
        try
        {
            string? username = _userDataDAL.GetUsername(userID);

            return username;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the username: " + ex.Message);
        }
    }

    public void UpdateUsername(string userID, string username)
    {
        try
        {
            _userDataDAL.UpdateUsername(userID, username);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the username: " + ex.Message);
        }
    }

    public int? GetUserRating(string userID)
    {
        try
        {
            int? rating = _userDataDAL.GetUserRating(userID);

            return rating;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the user rating: " + ex.Message);
        }
    }

    public UserDTO? GetUser(string userID)
    {
        try
        {
            UserDTO? userDTO = _userDataDAL.GetUser(userID);

            return userDTO;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the user: " + ex.Message);
        }
    }

    public byte[]? GetProfilePicture(string userID)
    {
        try
        {
            byte[]? profilePicture = _userDataDAL.GetProfilePicture(userID);
            
            return profilePicture;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the profile picture: " + ex.Message);
        }
    }

    public void UpdateProfilePicture(string userID, byte[] picture)
    {
        try
        {
            _userDataDAL.UpdateProfilePicture(userID, picture);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the profile picture: " + ex.Message);
        }
    }

}
