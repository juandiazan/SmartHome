using Domain;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace WebApi.Test;

[TestClass]
public class AuthenticationControllerTest
{
    [TestMethod]
    public void LogIn_WithCorrectData_ShouldReturnToken()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "username",
            Password = "password",
            Role = new Role { RoleName = "Admin" }
        };

        var loginRequest = new LoginRequest
        {
            Email = "email",
            Password = "password"
        };

        var sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        sessionService
            .Setup(s => s.Login(loginRequest.Email, loginRequest.Password))
            .Returns("valid-token");

        var authenticationController = new AuthenticationController(sessionService.Object);

        // Act
        var result = authenticationController.LogIn(loginRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void GetRoleOfUserByToken_WithCorrectData_ShouldOk()
    {
        var token = "auth";

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "username",
            Password = "password",
            Role = new Role { RoleName = "Admin" }
        };

        var sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        sessionService
            .Setup(s => s.GetUserByToken(token))
            .Returns(user);

        var authenticationController = new AuthenticationController(sessionService.Object);

        var result = authenticationController.GetRoleOfLoggedUser(token);

        result.Should().BeOfType<OkObjectResult>();
    }
}
