using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
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

    public async Task<string?> GetUsername(string userId)
    {

        return await _dbContext.User
            .Where(u => u.Id == userId)
            .Select(u => u.Username)
            .FirstOrDefaultAsync();
    }

    public async Task<int?> GetUserRating(string userId)
    {
        return await _dbContext.User
            .Where(u => u.Id == userId)
            .Select(u => u.Rating)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUser(string userId)
    {
        var userModel = await _dbContext.User
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (userModel == null)
            return null;

        var user = new User()
        {
            Username = userModel.Username,
            Rating = userModel.Rating
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

    public async Task<byte[]?> GetProfilePicture(string userId)
    {
        return await _dbContext.User
            .Where(u => u.Id == userId)
            .Select(u => u.ProfilePicture)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateProfilePicture(string userId, byte[] picture)
    {
        var user = await _dbContext.User
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
            return;

        var sql = "UPDATE `User` SET `ProfilePicture` = @picture WHERE `Id` = @userId";
        await _dbContext.Database.ExecuteSqlRawAsync(sql, new MySqlParameter("@picture", picture), new MySqlParameter("@userId", userId));
    }

    public async Task CreateUser(string userId, string username, byte[]? profilePicture)
    {
        var user = new UserModel()
        {
            Id = userId,
            Username = username,
            ProfilePicture = profilePicture
        };

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
}