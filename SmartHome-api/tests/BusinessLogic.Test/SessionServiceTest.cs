using Domain;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace BusinessLogic.Test;

[TestClass]
public class SessionServiceTest
{
    private SessionService _sessionService = null!;
    private Mock<ISessionRepository> _sessionRepository = null!;
    private Mock<IUserService> _userService = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _sessionRepository = new Mock<ISessionRepository>(MockBehavior.Strict);
        _userService = new Mock<IUserService>(MockBehavior.Strict);
        _sessionService = new SessionService(_sessionRepository.Object, _userService.Object);
    }

    [TestMethod]
    public void Login_WithCorrectCredentials_ShouldBeASessionWithTheUser()
    {
        var userEmail = "admin@gmail.com";
        var loggedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "admin",
            Surname = "admin",
            Email = "admin@gmail.com",
            Password = "Admin123!"
        };

        _userService
            .Setup(act => act.Exists(u => u.Email == userEmail))
            .Returns(true);

        _userService
            .Setup(act => act.IsPasswordCorrect(loggedUser.Email, loggedUser.Password))
            .Returns(true);

        _userService
            .Setup(act => act.GetUserByEmail(loggedUser.Email))
            .Returns(loggedUser);

        _sessionRepository
            .Setup(act => act.Add(It.IsAny<Session>()));

        _sessionRepository
            .Setup(act => act.HasActiveSessionByEmail(loggedUser.Email))
            .Returns(false);

        var resultantToken = _sessionService.Login(loggedUser.Email, loggedUser.Password);

        _sessionRepository
            .Setup(act => act.HasActiveSessionByEmail(loggedUser.Email))
            .Returns(true);

        var hasSession = _sessionService.HasActiveSessionByEmail("admin@gmail.com");

        hasSession.Should().BeTrue();
        resultantToken.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public void Login_WithNonMatchingPassword_ShouldThrowException()
    {
        var userEmail = "admin@gmail.com";
        var loggedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "admin",
            Surname = "admin",
            Email = "admin@gmail.com",
            Password = "Admin123!"
        };

        _userService
            .Setup(act => act.Exists(u => u.Email == userEmail))
            .Returns(true);

        _userService
            .Setup(act => act.IsPasswordCorrect(loggedUser.Email, "NonMatchingPass123!"))
            .Returns(false);

        var act = () => _sessionService.Login("admin@gmail.com", "NonMatchingPass123!");

        act.Should().Throw<ArgumentException>("Invalid password");
    }

    [TestMethod]
    public void Login_WithUserAlreadyLoggedIn_ShouldReturnSessionToken()
    {
        var expectedToken = "expectedToken";
        var userEmail = "admin@gmail.com";
        var loggedUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "admin",
            Surname = "admin",
            Email = userEmail,
            Password = "Admin123!"
        };

        _userService
            .Setup(act => act.Exists(u => u.Email == userEmail))
            .Returns(true);

        _userService
            .Setup(act => act.IsPasswordCorrect(loggedUser.Email, loggedUser.Password))
            .Returns(true);

        _sessionRepository
            .Setup(act => act.HasActiveSessionByEmail(loggedUser.Email))
            .Returns(true);

        _sessionRepository
            .Setup(act => act.GetSessionTokenByEmail(userEmail))
            .Returns(expectedToken);

        var token = _sessionService.Login(loggedUser.Email, loggedUser.Password);

        token.Should().Be(expectedToken);
    }

    [TestMethod]
    public void Login_WithNonExistentUserEmail_ShouldThrowException()
    {
        var nonExistentEmail = "invalidemail@gmail.com";

        _userService
            .Setup(act => act.Exists(u => u.Email == nonExistentEmail))
            .Returns(false);

        var act = () => _sessionService.Login(nonExistentEmail, It.IsAny<string>());

        act.Should().Throw<KeyNotFoundException>("User does not exist");
    }

    [TestMethod]
    public void IsAuthenticated_WithExistentToken_ShouldBeTrue()
    {
        var token = Guid.NewGuid().ToString();

        _sessionRepository
            .Setup(act => act.IsAuthenticated(token))
            .Returns(true);

        var isAuthenticated = _sessionService.IsAuthenticated(token);

        isAuthenticated.Should().BeTrue();
    }

    [TestMethod]
    public void GetUserByToken_WithCorrectUser_ShouldReturnUser()
    {
        var token = Guid.NewGuid().ToString();

        var result = new User
        {
            Name = "Name",
            Surname = "Surname",
            Email = "email@email.com",
            Password = "Password123!"
        };

        _sessionRepository
            .Setup(act => act.GetUserByToken(token))
            .Returns(result);

        var user = _sessionService.GetUserByToken(token);

        user.Should().NotBeNull();
    }

    [TestMethod]
    public void GetUserByToken_WithNonExistentUser_ShouldThrowKeyNotFoundException()
    {
        var token = Guid.NewGuid().ToString();

        _sessionRepository
            .Setup(act => act.GetUserByToken(token))
            .Returns((User?)null);

        var act = () => _sessionService.GetUserByToken(token);

        act.Should().Throw<KeyNotFoundException>("User is not in session");
    }
}
