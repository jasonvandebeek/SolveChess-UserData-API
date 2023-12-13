
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using SolveChess.Logic.DAL;
using SolveChess.Logic.Exceptions;
using SolveChess.Logic.Models;
using System.Net;

namespace SolveChess.Logic.Service.Tests;

[TestClass]
public class UserServiceTests
{

    [TestMethod]
    public async Task GetUsernameTest()
    {
        //Arrange
        var userdataDalMock = new Mock<IUserDataDal>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        userdataDalMock.Setup(dal => dal.GetUsername(It.IsAny<string>()))
            .ReturnsAsync("test");

        var service = new UserService(userdataDalMock.Object, httpClient);

        //Act
        var result = await service.GetUsername("123");

        //Assert
        Assert.AreEqual("test", result);
    }

    [TestMethod]
    public async Task GetUserRatingTest()
    {
        //Arrange
        var userdataDalMock = new Mock<IUserDataDal>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        userdataDalMock.Setup(dal => dal.GetUserRating(It.IsAny<string>()))
            .ReturnsAsync(100);

        var service = new UserService(userdataDalMock.Object, httpClient);

        //Act
        var result = await service.GetUserRating("123");

        //Assert
        Assert.AreEqual(100, result);
    }

    [TestMethod]
    public async Task GetUserTest_UserExists()
    {
        //Arrange
        var expected = new User { Username = "Test", Rating = 100 };

        var userdataDalMock = new Mock<IUserDataDal>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("123")
            });

        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        userdataDalMock.Setup(dal => dal.GetUser(It.IsAny<string>()))
            .ReturnsAsync(expected);

        var service = new UserService(userdataDalMock.Object, httpClient);

        //Act
        var result = await service.GetUser("123");

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task GetUserTest_UserDoesntExistAtAll()
    {
        //Arrange
        var userdataDalMock = new Mock<IUserDataDal>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        var service = new UserService(userdataDalMock.Object, httpClient);

        //Act
        var result = await service.GetUser("123");

        //Assert
        Assert.AreEqual(null, result);
    }

    [TestMethod]
    public async Task GetUserTest_UserDoesntExistIsCreated()
    {
        //Arrange
        var expected = new User { Username = "Test", Rating = 100 };

        var userdataDalMock = new Mock<IUserDataDal>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("123")
            });

        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        userdataDalMock.Setup(dal => dal.GetUser(It.IsAny<string>()))
            .ReturnsAsync(expected);

        var service = new UserService(userdataDalMock.Object, httpClient);

        //Act
        var result = await service.GetUser("123");

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task GetProfilePictureTest()
    {
        //Arrange
        var expected = new byte[] { 1 };

        var userdataDalMock = new Mock<IUserDataDal>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        userdataDalMock.Setup(dal => dal.GetProfilePicture(It.IsAny<string>()))
            .ReturnsAsync(expected);

        var service = new UserService(userdataDalMock.Object, httpClient);

        //Act
        var result = await service.GetProfilePicture("123");

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void CreateUserTest_GenerationAttemptFailureThrowsException()
    {
        //Arrange
        var userdataDalMock = new Mock<IUserDataDal>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        userdataDalMock.Setup(dal => dal.DoesUsernameExist(It.IsAny<string>()))
            .ReturnsAsync(true);

        var service = new UserService(userdataDalMock.Object, httpClient);

        //Assert
        Assert.ThrowsExceptionAsync<UsernameGenerationException>(async() =>
        {
            //Act
            await service.CreateUser("123", null, null);
        });
    }

}
