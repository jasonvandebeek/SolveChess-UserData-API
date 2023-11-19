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

    public string? GetUsername(string userId)
    {
        try
        {
            string? username = _userDataDAL.GetUsername(userId);

            return username;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the username: " + ex.Message);
        }
    }

    public void UpdateUsername(string userId, string username)
    {
        try
        {
            _userDataDAL.UpdateUsername(userId, username);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the username: " + ex.Message);
        }
    }

    public int? GetUserRating(string userId)
    {
        try
        {
            int? rating = _userDataDAL.GetUserRating(userId);

            return rating;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the user rating: " + ex.Message);
        }
    }

    public UserDTO? GetUser(string userId)
    {
        try
        {
            UserDTO? userDTO = _userDataDAL.GetUser(userId);

            return userDTO;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the user: " + ex.Message);
        }
    }

    public byte[]? GetProfilePicture(string userId)
    {
        try
        {
            byte[]? profilePicture = _userDataDAL.GetProfilePicture(userId);
            
            return profilePicture;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the profile picture: " + ex.Message);
        }
    }

    public void UpdateProfilePicture(string userId, byte[] picture)
    {
        try
        {
            _userDataDAL.UpdateProfilePicture(userId, picture);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the profile picture: " + ex.Message);
        }
    }

    public void CreateUser(string userId, string username, byte[]? picture)
    {
        try
        {
            _userDataDAL.CreateUser(userId, username, picture);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating a new user: " + ex.Message);
        }
    }
}
