using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SolveChess.DAL.Model;
using SolveChess.Logic.DAL;
using SolveChess.Logic.DTO;

namespace SolveChess.DAL;

public class UserDataDAL : IUserDataDAL
{

    private readonly AppDbContext _dbContext;

    public UserDataDAL(DbContextOptions<AppDbContext> options)
    {
        _dbContext = new AppDbContext(options);
    }

    public string? GetUsername(string userID)
    {

        return _dbContext.User
            .Where(u => u.Id == userID)
            .Select(u => u.Username)
            .FirstOrDefault();
    }

    public int? GetUserRating(string userID)
    {
        return _dbContext.User
            .Where(u => u.Id == userID)
            .Select(u => u.Rating)
            .FirstOrDefault();
    }

    public UserDTO? GetUser(string userID)
    {
        var user = _dbContext.User
            .Where(u => u.Id == userID)
            .FirstOrDefault();

        if (user == null)
            return null;

        var userDTO = new UserDTO()
        {
            Username = user.Username,
            Rating = user.Rating,
        };

        return userDTO;
    }

    public void UpdateUsername(string userID, string newUsername)
    {
        var user = _dbContext.User
            .Where(u => u.Id == userID)
            .FirstOrDefault();

        if(user == null)
            return;

        user.Username = newUsername;

        _dbContext.SaveChanges();
    }

    public byte[]? GetProfilePicture(string userID)
    {
        return _dbContext.User
            .Where(u => u.Id == userID)
            .Select(u => u.ProfilePicture)
            .FirstOrDefault();
    }

    public void UpdateProfilePicture(string userID, byte[] picture)
    {
        var user = _dbContext.User
            .Where(u => u.Id == userID)
            .FirstOrDefault();

        if (user == null)
            return;

        var sql = "UPDATE `User` SET `ProfilePicture` = @picture WHERE `Id` = @userID";
        _dbContext.Database.ExecuteSqlRaw(sql, new MySqlParameter("@picture", picture), new MySqlParameter("@userID", userID));

        //user.ProfilePicture = picture;
        //_dbContext.SaveChanges();
    }

}