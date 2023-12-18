using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SolveChess.API.IntegrationTests;
using SolveChess.DAL.Model;
using SolveChess.IntegrationTests.Helpers;
using SolveChess.Logic.Models;
using System.Net;
using System.Text;

namespace SolveChess.API.Controllers.Tests;

[TestClass]
public class UsersControllerTests
{

    private SolveChessWebApplicationFactory _factory = null!;
    private AppDbContext _dbContext = null!;

    private readonly User _user = new() { Id = "123", Username = "test", Rating = 100, ProfilePicture = new byte[] { 1 } };

    [TestInitialize]
    public void TestInitialize()
    {
        _factory = new SolveChessWebApplicationFactory();

        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    [TestMethod]
    public async Task GetUserByIdTest_Returns200OkAndUser()
    {
        //Arrange
        var userId = _user.Id;

        var client = _factory.CreateClient();

        var json = new
        {
            userId,
            username = _user.Username,
            rating = _user.Rating,
            profilePictureUrl = $"{client.BaseAddress}Users/{userId}/profile-picture"
        };
        string expected = JsonConvert.SerializeObject(json, Formatting.None);

        _dbContext.User.Add(_user);
        _dbContext.SaveChanges();

        //Act
        var response = await client.GetAsync($"/users/{userId}");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(expected, await response.Content.ReadAsStringAsync());
    }

    [TestMethod]
    public async Task GetUserByUsernameTest_Returns200OkAndUser()
    {
        //Arrange
        var client = _factory.CreateClient();

        var json = new
        {
            userId = _user.Id,
            username = _user.Username,
            rating = _user.Rating,
            profilePictureUrl = $"{client.BaseAddress}Users/{_user.Id}/profile-picture"
        };
        string expected = JsonConvert.SerializeObject(json, Formatting.None);

        _dbContext.User.Add(_user);
        _dbContext.SaveChanges();

        //Act
        var response = await client.GetAsync($"/users?username={_user.Username}");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(expected, await response.Content.ReadAsStringAsync());
    }


    [TestMethod]
    public async Task CreateUserTest_Returns201CreatedWithUsername()
    {
        //Arrange
        var userId = _user.Id;
        var jwtToken = JwtTokenHelper.GenerateTestToken(userId);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={jwtToken}");

        var content = new MultipartFormDataContent
        {
            { new StringContent("test"), "Username" },
            { new StringContent(string.Empty), "ProfilePicture" }
        };

        //Act
        var response = await client.PostAsync("/users", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
        Assert.IsNotNull(user);
        Assert.AreEqual(_user.Username, user.Username);
    }

    [TestMethod]
    public async Task CreateUserTest_Returns201CreatedWithoutUsername()
    {
        //Arrange
        var userId = _user.Id;
        var jwtToken = JwtTokenHelper.GenerateTestToken(userId);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={jwtToken}");

        var content = new MultipartFormDataContent
        {
            { new StringContent(string.Empty), "Username" },
            { new StringContent(string.Empty), "ProfilePicture" }
        };

        //Act
        var response = await client.PostAsync("/users", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
        Assert.IsNotNull(user);
    }

    [TestMethod]
    public async Task CreateUserTest_Returns401Unauthorized()
    {
        //Arrange
        var client = _factory.CreateClient();

        var content = new MultipartFormDataContent
        {
            { new StringContent("test"), "Username" },
            { new StringContent(string.Empty), "ProfilePicture" }
        };

        //Act
        var response = await client.PostAsync("/users", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task GetUsernameByIdTest_Returns200OkAndUsername()
    {
        //Arrange
        var userId = _user.Id;

        var client = _factory.CreateClient();

        _dbContext.User.Add(_user);
        _dbContext.SaveChanges();

        //Act
        var response = await client.GetAsync($"/users/{userId}/username");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(_user.Username, await response.Content.ReadAsStringAsync());
    }

    [TestMethod]
    public async Task GetRatingByIdTest_Returns200OkAndRating()
    {
        //Arrange
        var userId = _user.Id;

        var client = _factory.CreateClient();

        _dbContext.User.Add(_user);
        _dbContext.SaveChanges();

        //Act
        var response = await client.GetAsync($"/users/{userId}/rating");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(_user.Rating.ToString(), await response.Content.ReadAsStringAsync());
    }

    [TestMethod]
    public async Task GetProfilePictureByIdTest_Returns200OkAndProfilePicture()
    {
        //Arrange
        var userId = _user.Id;

        var client = _factory.CreateClient();

        _dbContext.User.Add(_user);
        _dbContext.SaveChanges();

        //Act
        var response = await client.GetAsync($"/users/{userId}/profile-picture");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("image/png", response.Content.Headers.ContentType?.MediaType);
    }

    [TestMethod]
    public async Task UpdateUsernameTest_Returns200Ok()
    {
        //Arrange
        var userId = _user.Id;
        var jwtToken = JwtTokenHelper.GenerateTestToken(userId);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={jwtToken}");

        var content = new StringContent("\"newTest\"", Encoding.UTF8, "application/json");

        _dbContext.User.Add(_user);
        _dbContext.SaveChanges();

        //Act
        var response = await client.PutAsync($"/users/{userId}/username", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
        Assert.IsNotNull(user);
        _dbContext.Entry(user).Reload();

        Assert.AreEqual("newTest", user.Username);
    }

    [TestMethod]
    public async Task UpdateUsernameTest_EditingOtherUserReturns403Forbidden()
    {
        //Arrange
        var userId = _user.Id;
        var jwtToken = JwtTokenHelper.GenerateTestToken(userId);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={jwtToken}");

        var content = new StringContent("\"newTest\"", Encoding.UTF8, "application/json");

        //Act
        var response = await client.PutAsync($"/users/{500}/username", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [TestMethod]
    public async Task UpdateProfilePictureTest_Returns200Ok()
    {
        //Arrange
        var userId = _user.Id;
        var jwtToken = JwtTokenHelper.GenerateTestToken(userId);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={jwtToken}");

        var content = new MultipartFormDataContent
        {
            { new ByteArrayContent( new byte[] { 2 }), "picture", "test.png" }
        };

        _dbContext.User.Add(_user);
        _dbContext.SaveChanges();

        //Act
        var response = await client.PutAsync($"/users/{userId}/profile-picture", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
        Assert.IsNotNull(user);
        _dbContext.Entry(user).Reload();

        CollectionAssert.AreEqual(new byte[] { 2 }, user.ProfilePicture);
    }

    [TestMethod]
    public async Task UpdateProfilePictureTest_EditingOtherUserReturns403Forbidden()
    {
        //Arrange
        var userId = _user.Id;
        var jwtToken = JwtTokenHelper.GenerateTestToken(userId);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={jwtToken}");

        var content = new MultipartFormDataContent
        {
            { new ByteArrayContent( new byte[] { 2 }), "picture", "test.png" }
        };

        //Act
        var response = await client.PutAsync($"/users/{500}/profile-picture", content);

        //Assert
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        var entitiesToRemove = _dbContext.Set<User>().ToList();
        _dbContext.Set<User>().RemoveRange(entitiesToRemove);
        _dbContext.SaveChanges();
    }

}
