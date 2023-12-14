using SolveChess.Logic.DAL;
using SolveChess.Logic.Models;
using SolveChess.Logic.Interfaces;
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

    public async Task<User?> GetUser(string userId)
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

    public async Task CreateUser(string userId, string? username, byte[]? picture)
    {
        username ??= await GetRandomUsername(3);

        var user = new User
        {
            Id = userId,
            Username = username,
            Rating = 300,
            ProfilePicture = picture
        };

        await _userDataDal.CreateUser(user);
    }

    public async Task UpdateUsername(string userId, string username)
    {
        await _userDataDal.UpdateUsername(userId, username);
    }

    public async Task UpdateProfilePicture(string userId, byte[] picture)
    {
        await _userDataDal.UpdateProfilePicture(userId, picture);
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
