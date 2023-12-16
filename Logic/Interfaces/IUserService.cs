
using SolveChess.Logic.Models;

namespace SolveChess.Logic.Interfaces;

public interface IUserService
{
    
    public Task<User?> GetUserById(string userId);
    public Task<User?> GetUserByUsername(string username);
    public Task<User> CreateUser(string userId, string? username, byte[]? picture);
    public Task UpdateUsername(string userId, string username);
    public Task UpdateProfilePicture(string userId, byte[] picture);

}
