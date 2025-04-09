using Domain;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.Requests;

namespace WebApi.Test;

[TestClass]
public class HomeOwnerControllerTest
{
    [TestMethod]
    public void CreateHomeOwner_WithCorrectData_ShouldReturnOk()
    {
        // Arrange
        var mockService = new Mock<IHomeOwnerService>();
        var controller = new HomeOwnerController(mockService.Object);

        var newHomeOwner = new HomeOwner
        {
            Name = "John Doe",
            Surname = "Doe",
            Email = "Email@gmail.com",
            Password = "Password123!",
            ProfilePicture = "profilePicture",
            Role = new Role
            {
                Id = Guid.NewGuid(),
                RoleName = "company-owner"
            },
        };

        var ownerRequest = new CreateHomeOwnerRequest(
            newHomeOwner.Name,
            newHomeOwner.Surname,
            newHomeOwner.ProfilePicture,
            newHomeOwner.Email,
            newHomeOwner.Password);

        mockService
            .Setup(service => service.Create(ownerRequest.ToArgs()))
            .Returns(newHomeOwner);

        // Act
        var result = controller.CreateHomeOwner(ownerRequest);

        // Assert
        result.Should().BeOfType<CreatedResult>();
    }

    [TestMethod]
    public void GetHomeOwnerOwnedHomeId_WithCorrectData_ShouldReturnOk()
    {
        // Arrange
        var mockService = new Mock<IHomeOwnerService>();
        var controller = new HomeOwnerController(mockService.Object);

        var homeOwnerId = Guid.NewGuid();
        var token = "token";

        mockService
            .Setup(service => service.GetHomeOwnerOwnedHomeId(token))
            .Returns(homeOwnerId.ToString());

        // Act
        var result = controller.GetHomeOwnerOwnedHomeId(token);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}
