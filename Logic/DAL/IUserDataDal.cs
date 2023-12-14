using SolveChess.Logic.Models;

namespace SolveChess.Logic.DAL;

public interface IUserDataDal
{

    public Task UpdateUsername(string userId, string newUsername);
    public Task<User?> GetUser(string userId);
    public Task UpdateProfilePicture(string userId, byte[] picture);
    public Task CreateUser(User user);
    public Task<bool> DoesUsernameExist(string username);

}