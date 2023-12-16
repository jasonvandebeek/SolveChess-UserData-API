using SolveChess.Logic.Models;

namespace SolveChess.Logic.DAL;

public interface IUserDataDal
{

    public Task UpdateUsername(string userId, string newUsername);
    public Task<User?> GetUserById(string userId);
    public Task<User?> GetUserByUsername(string username);
    public Task UpdateProfilePicture(string userId, byte[] picture);
    public Task CreateUser(User user);
    public Task<bool> DoesUsernameExist(string username);

}