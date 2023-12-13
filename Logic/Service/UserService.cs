using SolveChess.Logic.DAL;
using SolveChess.Logic.Models;
using SolveChess.Logic.Interfaces;
using System;
using SolveChess.Logic.Helpers;
using SolveChess.Logic.Exceptions;

namespace SolveChess.Logic.Service;

public class UserService : IUserService
{

    private readonly IUserDataDal _userDataDal;
    private readonly HttpClient _httpClient;

    public UserService(IUserDataDal userDataDal, HttpClient httpClient)
    {
        _userDataDal = userDataDal;
        _httpClient = httpClient;
    }

    public async Task<string?> GetUsername(string userId)
    {
        try
        {
            string? username = await _userDataDal.GetUsername(userId);

            return username;
        }
        catch (Exception exception)
        {
            throw new Exception("An error occurred while retrieving the username!", exception);
        }
    }

    public async Task UpdateUsername(string userId, string username)
    {
        try
        {
            await _userDataDal.UpdateUsername(userId, username);
        }
        catch (Exception exception)
        {
            throw new Exception("An error occurred while updating the username!", exception);
        }
    }

    public async Task<int?> GetUserRating(string userId)
    {
        try
        {
            int? rating = await _userDataDal.GetUserRating(userId);

            return rating;
        }
        catch (Exception exception)
        {
            throw new Exception("An error occurred while retrieving the user rating!", exception);
        }
    }

    public async Task<User?> GetUser(string userId)
    {
        try
        {
            User? user = await _userDataDal.GetUser(userId);
            if(user != null)
                return user;

            var response = await _httpClient.GetAsync($"https://api.solvechess.xyz/auth/user/{userId}");
            if (!response.IsSuccessStatusCode)
                return null;

            await CreateUser(userId, null, null);
            return await _userDataDal.GetUser(userId);
        }
        catch (Exception exception)
        {
            throw new Exception("An error occurred while retrieving the user!", exception);
        }
    }

    public async Task<byte[]?> GetProfilePicture(string userId)
    {
        try
        {
            byte[]? profilePicture = await _userDataDal.GetProfilePicture(userId);
            
            return profilePicture;
        }
        catch (Exception exception)
        {
            throw new Exception("An error occurred while retrieving the profile picture!", exception);
        }
    }

    public async Task UpdateProfilePicture(string userId, byte[] picture)
    {
        try
        {
            await _userDataDal.UpdateProfilePicture(userId, picture);
        }
        catch (Exception exception)
        {
            throw new Exception("An error occurred while updating the profile picture!", exception);
        }
    }

    public async Task CreateUser(string userId, string? username, byte[]? picture)
    {
        username ??= await GetRandomUsername(3);

        try
        {
            await _userDataDal.CreateUser(userId, username, picture);
        }
        catch (Exception exception)
        {
            throw new Exception("An error occurred while creating a new user!", exception);
        }
    }

    private async Task<string> GetRandomUsername(int maxAttemps)
    {
        if (maxAttemps <= 0)
            throw new UsernameGenerationException();

        var username = UsernameHelper.GetRandomUsername();
        if (await _userDataDal.DoesUsernameExist(username))
            return await GetRandomUsername(maxAttemps - 1);

        return username;
    }

}
