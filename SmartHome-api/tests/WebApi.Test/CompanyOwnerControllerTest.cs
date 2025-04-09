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
public class CompanyOwnerControllerTest
{
    [TestMethod]
    public void Create_WithCorrectData_ShouldCreate()
    {
        var args = new CreateCompanyOwnerRequest(
            "Name",
            "Surname",
            "email@email.com",
            "@Passw1rd");

        var expectedCompanyOwner = new CompanyOwner
        {
            Name = args.Name,
            Surname = args.Surname,
            Email = args.Email,
            Password = args.Password,
            RoleId = Guid.NewGuid(),
            Role = new Role
            {
                Id = Guid.NewGuid(),
                RoleName = "CompanyOwner"
            },
            AccountState = false,
        };

        var companyOwnerService = new Mock<ICompanyOwnerService>(MockBehavior.Strict);
        var sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        companyOwnerService
            .Setup(act => act.Create(It.IsAny<CreateUserArgs>()))
            .Returns(expectedCompanyOwner);

        var companyOwnerController = new CompanyOwnerController(companyOwnerService.Object, sessionService.Object);

        var result = companyOwnerController.CreateCompanyOwner(args);

        result.Should().BeOfType<CreatedResult>();
    }

    [TestMethod]
    public void UpdateRole_WithCorrectData_ShouldUpdate()
    {
        var userId = Guid.NewGuid();
        var token = "token";

        var userBeforeModification = new CompanyOwner
        {
            Id = userId,
            Name = "Test",
            Surname = "User",
            Email = "email@gmail.com",
            Password = "Password123!",
            Role = new Role { RoleName = "company-owner" }
        };

        var user = new CompanyOwner
        {
            Id = userId,
            Name = "Test",
            Surname = "User",
            Email = "email@gmail.com",
            Password = "Password123!",
            Role = new Role { RoleName = "company-owner-home-owner" }
        };

        var companyOwnerService = new Mock<ICompanyOwnerService>(MockBehavior.Strict);
        var sessionService = new Mock<ISessionService>(MockBehavior.Strict);

        sessionService
            .Setup(act => act.GetUserByToken(token))
            .Returns(userBeforeModification);

        companyOwnerService
            .Setup(act => act.GiveHomeOwnerRoleToCompanyOwner(userId))
            .Returns(user);

        var companyOwnerController = new CompanyOwnerController(companyOwnerService.Object, sessionService.Object);

        var result = companyOwnerController.GiveHomeOwnerPermissionsToCompanyOwner(token);

        result.Should().BeOfType<OkObjectResult>();
    }
}
