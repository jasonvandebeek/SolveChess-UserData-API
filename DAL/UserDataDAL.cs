using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SolveChess.DAL.Model;
using SolveChess.Logic.DAL;
using SolveChess.Logic.DTO;
using System;

namespace SolveChess.DAL;

public class UserDataDAL : IUserDataDAL
{

    private readonly AppDbContext _dbContext;

    public UserDataDAL(DbContextOptions<AppDbContext> options)
    {
        _dbContext = new AppDbContext(options);
    }

    public string? GetUsername(string userId)
    {

        return _dbContext.User
            .Where(u => u.Id == userId)
            .Select(u => u.Username)
            .FirstOrDefault();
    }

    public int? GetUserRating(string userId)
    {
        return _dbContext.User
            .Where(u => u.Id == userId)
            .Select(u => u.Rating)
            .FirstOrDefault();
    }

    public UserDTO? GetUser(string userId)
    {
        var user = _dbContext.User
            .Where(u => u.Id == userId)
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

    public void UpdateUsername(string userId, string newUsername)
    {
        var user = _dbContext.User
            .Where(u => u.Id == userId)
            .FirstOrDefault();

        if(user == null)
            return;

        user.Username = newUsername;

        _dbContext.SaveChanges();
    }

    public byte[]? GetProfilePicture(string userId)
    {
        return _dbContext.User
            .Where(u => u.Id == userId)
            .Select(u => u.ProfilePicture)
            .FirstOrDefault();
    }

    public void UpdateProfilePicture(string userId, byte[] picture)
    {
        var user = _dbContext.User
            .Where(u => u.Id == userId)
            .FirstOrDefault();

        if (user == null)
            return;

        var sql = "UPDATE `User` SET `ProfilePicture` = @picture WHERE `Id` = @userId";
        _dbContext.Database.ExecuteSqlRaw(sql, new MySqlParameter("@picture", picture), new MySqlParameter("@userId", userId));

        //user.ProfilePicture = picture;
        //_dbContext.SaveChanges();
    }

    public void CreateUser(string userId, string username, byte[]? profilePicture)
    {
        var user = new User()
        {
            Id = userId,
            Username = username,
            ProfilePicture = profilePicture
        };

        _dbContext.User.Add(user);
        _dbContext.SaveChanges();
    }
}