using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.Requests;

namespace WebApi.Test;

[TestClass]
public class AdministratorControllerTest
{
    private Mock<IUserService> _administratorService = null!;
    private Mock<ISessionService> _sessionService = null!;
    private AdminController _administratorController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _administratorService = new Mock<IUserService>(MockBehavior.Strict);
        _sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        _administratorController = new AdminController(_administratorService.Object, _sessionService.Object);
    }

    [TestMethod]
    public void CreateAdmin_WithCorrectData_ShouldCreate()
    {
        var request = new CreateUserRequest(
            "Name",
            "Surname",
            "email@email.com",
            "@Passw1rd");

        var args = request.ToArgs();

        var expectedAdministrator = new User
        {
            Name = args.Name,
            Surname = args.Surname,
            Email = args.Email,
            Password = args.Password,
            RoleId = Guid.NewGuid(),
            Role = new Role
            {
                Id = Guid.NewGuid(),
                RoleName = "Administrator"
            }
        };

        _administratorService
            .Setup(act => act.Create(It.IsAny<CreateUserArgs>()))
            .Returns(expectedAdministrator);

        var result = _administratorController.CreateAdministrator(request);

        result.Should().BeOfType<CreatedResult>();
    }

    [TestMethod]
    public void CreateAdmin_WithNullName_ShouldBadRequest()
    {
        var request = new CreateUserRequest(
            null,
            "Surname",
            "email@email.com",
            "@Passw1rd");

        _administratorService
            .Setup(act => act.Create(It.IsAny<CreateUserArgs>()))
            .Throws(new ArgumentNullException(nameof(CreateUserArgs.Name), "Administrator name cannot be null or empty"));

        var action = () => _administratorController.CreateAdministrator(request);
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void DeleteAdmin_WithCorrectData_ShouldDelete()
    {
        var userRetornado = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Surname = "User",
            Email = "testuser@example.com",
            Password = "password",
            RoleId = Guid.NewGuid(),
            Role = new Role
            {
                Id = Guid.NewGuid(),
                RoleName = "Administrator"
            }
        };

        _administratorService
            .Setup(act => act.DeleteById(userRetornado.Id))
            .Returns(userRetornado);

        var result = _administratorController.DeleteAdministrator(userRetornado.Id);

        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public void DeleteAdmin_WithNonExistentId_ShouldReturnNotFound()
    {
        var userId = Guid.NewGuid();

        _administratorService
            .Setup(act => act.DeleteById(userId))
            .Throws(new KeyNotFoundException("User to delete does not exist"));

        Action act = () => _administratorController.DeleteAdministrator(userId);
        act.Should().Throw<KeyNotFoundException>();
    }

    [TestMethod]
    public void Create_WithInvalidEmail_ShouldReturnBadRequest()
    {
        var args = new CreateUserRequest(
            "Name",
            "Surname",
            "invalid-email",
            "@Passw1rd");

        Action act = () => _administratorController.CreateAdministrator(args);
        act.Should().Throw<FormatException>();
    }

    [TestMethod]
    public void Create_WithMissingPassword_ShouldReturnBadRequest()
    {
        var args = new CreateUserRequest(
            "Name",
            "Surname",
            "email@email.com",
            null);

        Action act = () => _administratorController.CreateAdministrator(args);
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void UpdateRole_WithCorrectData_ShouldUpdate()
    {
        var userId = Guid.NewGuid();
        var token = "token";

        var userBeforeModification = new User
        {
            Id = userId,
            Name = "Test",
            Surname = "User",
            Email = "email@gmail.com",
            Password = "Password123!",
            Role = new Role { RoleName = "administrator" }
        };

        var user = new User
        {
            Id = userId,
            Name = "Test",
            Surname = "User",
            Email = "email@gmail.com",
            Password = "Password123!",
            Role = new Role { RoleName = "administrator-home-owner" }
        };

        var userService = new Mock<IUserService>(MockBehavior.Strict);
        var sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        sessionService
            .Setup(act => act.GetUserByToken(token))
            .Returns(userBeforeModification);

        userService
            .Setup(act => act.UpdateRoleOfAdministratorToHomeOwner(userId))
            .Returns(user);

        var userController = new AdminController(userService.Object, sessionService.Object);

        var result = userController.GiveHomeOwnerPermissionsToAdmin(token);

        result.Should().BeOfType<OkObjectResult>();
    }
}
