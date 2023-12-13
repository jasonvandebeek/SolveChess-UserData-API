
using SolveChess.Logic.Models;

namespace SolveChess.Logic.Interfaces;

public interface IUserService
{

    public Task<string?> GetUsername(string userId);
    public Task UpdateUsername(string userId, string username);
    public Task<int?> GetUserRating(string userId);
    public Task<User?> GetUser(string userId);
    public Task<byte[]?> GetProfilePicture(string userId);
    public Task UpdateProfilePicture(string userId, byte[] picture);
    public Task CreateUser(string userId, string? username, byte[]? picture);

}
