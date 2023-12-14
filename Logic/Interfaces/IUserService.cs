
using SolveChess.Logic.Models;

namespace SolveChess.Logic.Interfaces;

public interface IUserService
{
    
    public Task<User?> GetUser(string userId);
    public Task CreateUser(string userId, string? username, byte[]? picture);
    public Task UpdateUsername(string userId, string username);
    public Task UpdateProfilePicture(string userId, byte[] picture);

}
