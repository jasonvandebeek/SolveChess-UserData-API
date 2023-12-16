
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
    public async Task GetUserByIdTest_UserExists()
    {
        //Arrange
        var expected = new User { Id = "123", Username = "Test", Rating = 100 };

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

        userdataDalMock.Setup(dal => dal.GetUserById(It.IsAny<string>()))
            .ReturnsAsync(expected);

        var service = new UserService(userdataDalMock.Object, httpClient);

        //Act
        var result = await service.GetUserById("123");

        //Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task GetUserByIdTest_UserDoesntExistAtAll()
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
        var result = await service.GetUserById("123");

        //Assert
        Assert.AreEqual(null, result);
    }

    [TestMethod]
    public async Task GetUserByIdTest_UserDoesntExistIsCreated()
    {
        //Arrange
        var expected = new User { Id = "123", Username = "Test", Rating = 100 };

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

        userdataDalMock.Setup(x => x.GetUserById(It.IsAny<string>()))
            .ReturnsAsync(null as User);

        var service = new UserService(userdataDalMock.Object, httpClient);

        //Act
        var result = await service.GetUserById("123");
        Assert.IsNotNull(result);

        //Assert
        Assert.AreEqual(expected.Id, result.Id);
    }

    [TestMethod]
    public async Task GetUserByUsernameTest()
    {
        //Arrange
        var expected = new User { Id = "123", Username = "Test", Rating = 100 };

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

        userdataDalMock.Setup(dal => dal.GetUserByUsername(It.IsAny<string>()))
            .ReturnsAsync(expected);

        var service = new UserService(userdataDalMock.Object, httpClient);

        //Act
        var result = await service.GetUserByUsername("123");

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
