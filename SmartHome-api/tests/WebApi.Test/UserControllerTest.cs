using DTOs;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaginationAndFilters.Models;
using WebApi.Controllers;
using WebApi.Models.Queries;

namespace WebApi.Test;

[TestClass]
public class UserControllerTest
{
    [TestMethod]
    public void GetAllUsers_WithCorrectData_ShouldReturnOkResult()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        var args = new UserFilterArgs(1, 10, "Admin", "John");
        var query = new GetUsersQuery(1, 10, "Admin", "John");

        var users = new List<GetAllUserArgs>
            {
                new("Id", "John", "Dan", "John Doe", "Admin", DateTime.Now.ToString()),
                new("Id", "John", "Doe", "Jane Doe", "Admin", DateTime.Now.ToString())
            };

        userServiceMock.Setup(service => service.GetAll(args))
            .Returns(users);

        var userController = new UserController(userServiceMock.Object);

        // Act
        var result = userController.GetAllUsers(query);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(users);
    }

    [TestMethod]
    public void GetAllUsers_WithIncorrectData_ShouldReturnBadRequestResult()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        var args = new UserFilterArgs(-10, 10, null, null);
        var query = new GetUsersQuery(-10, 10, null, null);

        userServiceMock
            .Setup(service => service.GetAll(args))
            .Throws(new ArgumentException("Page number cannot be negative"));

        var userController = new UserController(userServiceMock.Object);

        // Act
        Action act = () => userController.GetAllUsers(query);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Page number cannot be negative");
    }
}
