using Microsoft.EntityFrameworkCore;
using SolveChess.DAL.Model;
using SolveChess.Logic.DAL;
using SolveChess.Logic.Models;

namespace SolveChess.DAL;

public class UserDataDal : IUserDataDal
{

    private readonly AppDbContext _dbContext;

    public UserDataDal(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserById(string userId)
    {
        var userModel = await _dbContext.User
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (userModel == null)
            return null;

        var user = new User()
        {
            Id = userId,
            Username = userModel.Username,
            Rating = userModel.Rating,
            ProfilePicture = userModel.ProfilePicture
        };

        return user;
    }

    public async Task UpdateUsername(string userId, string newUsername)
    {
        var user = await _dbContext.User
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if(user == null)
            return;

        user.Username = newUsername;

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProfilePicture(string userId, byte[] picture)
    {
        var user = await _dbContext.User
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
            return;

        user.ProfilePicture = picture;
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateUser(User user)
    {
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DoesUsernameExist(string username)
    {
        var user = await _dbContext.User.Where(u => u.Username == username).FirstOrDefaultAsync();

        if(user == null) 
            return false;

        return true;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        var userModel = await _dbContext.User
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();

        if (userModel == null)
            return null;

        var user = new User()
        {
            Id = userModel.Id,
            Username = username,
            Rating = userModel.Rating,
            ProfilePicture = userModel.ProfilePicture
        };

        return user;
    }
}