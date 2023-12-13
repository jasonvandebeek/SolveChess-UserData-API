using SolveChess.Logic.Models;

namespace SolveChess.Logic.DAL;

public interface IUserDataDal
{

    public Task<string?> GetUsername(string userId);
    public Task UpdateUsername(string userId, string newUsername);
    public Task<int?> GetUserRating(string userId);
    public Task<User?> GetUser(string userId);
    public Task<byte[]?> GetProfilePicture(string userId);
    public Task UpdateProfilePicture(string userId, byte[] picture);
    public Task CreateUser(string userId, string username, byte[]? profilePicture);
    public Task<bool> DoesUsernameExist(string username);

}