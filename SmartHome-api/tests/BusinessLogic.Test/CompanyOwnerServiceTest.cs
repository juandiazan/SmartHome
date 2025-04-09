using Domain;
using DTOs;
using FluentAssertions;
using IDataAccess;
using Moq;

namespace BusinessLogic.Test;

[TestClass]
public class CompanyOwnerServiceTest
{
    private Mock<ICompanyOwnerRepository> _companyOwnerRepository = null!;

    private CompanyOwnerService _companyOwnerService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _companyOwnerRepository = new Mock<ICompanyOwnerRepository>(MockBehavior.Strict);
        _companyOwnerService = new CompanyOwnerService(_companyOwnerRepository.Object);
    }

    #region Create
    #region Error
    [TestMethod]
    public void Create_WithAlreadyExistentEmail_ShouldThrowCompanyOwnerServiceException()
    {
        var newCompanyOwner = new CreateUserArgs(
            "CompanyOwnerName",
            "CompanyOwnerSurname",
            "CompanyOwnerEmail@gmail.com",
            "CompanyOwnerPassword123!");

        _companyOwnerRepository
            .Setup(act => act.Exists(co => co.Email == newCompanyOwner.Email))
            .Returns(true);

        var act = () => _companyOwnerService.Create(newCompanyOwner);

        act.Should().Throw<InvalidOperationException>("A user with the entered email has already been registered");
    }
    #endregion
    #region Success
    [TestMethod]
    public void Create_WithCorrectData_ShouldCreateSuccesfullyAndHaveCorrectData()
    {
        var newCompanyOwner = new CreateUserArgs(
            "CompanyOwnerName",
            "CompanyOwnerSurname",
            "CompanyOwnerEmail@gmail.com",
            "CompanyOwnerPassword123!");

        var companyOwnerRoleId = Guid.NewGuid();

        _companyOwnerRepository
            .Setup(act => act.GetCompanyOwnerRoleId())
            .Returns(companyOwnerRoleId);

        _companyOwnerRepository
            .Setup(act => act.GetRoleById(companyOwnerRoleId))
            .Returns(new Role { RoleName = "company-owner" });

        _companyOwnerRepository
            .Setup(act => act.Add(It.Is<CompanyOwner>(co =>
            co.Name == newCompanyOwner.Name &&
            co.Surname == newCompanyOwner.Surname &&
            co.Email == newCompanyOwner.Email &&
            co.Password == newCompanyOwner.Password &&
            co.RoleId == companyOwnerRoleId)))
            .Returns(It.Is<CompanyOwner>(co => co.Email == newCompanyOwner.Email));

        _companyOwnerRepository
            .Setup(act => act.Exists(co => co.Email == newCompanyOwner.Email))
            .Returns(false);

        var result = _companyOwnerService.Create(newCompanyOwner);

        result.Name.Should().Be(newCompanyOwner.Name);
        result.Surname.Should().Be(newCompanyOwner.Surname);
        result.Email.Should().Be(newCompanyOwner.Email);
        result.Password.Should().Be(newCompanyOwner.Password);
        result.CreationDate.Date.Should().Be(DateTime.Today);
        result.RoleId.Should().Be(companyOwnerRoleId);
        result.Role.RoleName.Should().Be("company-owner");
    }

    [TestMethod]
    public void Create_WithCorrectData_ShouldCreateNotActiveAccount()
    {
        var newCompanyOwner = new CreateUserArgs(
            "CompanyOwnerName",
            "CompanyOwnerSurname",
            "CompanyOwnerEmail@gmail.com",
            "CompanyOwnerPassword123!");

        var companyOwnerRoleId = Guid.NewGuid();

        _companyOwnerRepository
            .Setup(act => act.GetCompanyOwnerRoleId())
            .Returns(companyOwnerRoleId);

        _companyOwnerRepository
            .Setup(act => act.GetRoleById(companyOwnerRoleId))
            .Returns(new Role { RoleName = "company-owner" });

        _companyOwnerRepository
            .Setup(act => act.Exists(co => co.Email == newCompanyOwner.Email))
            .Returns(false);

        _companyOwnerRepository
            .Setup(act => act.Add(It.Is<CompanyOwner>(co =>
            co.Name == newCompanyOwner.Name &&
            co.Surname == newCompanyOwner.Surname &&
            co.Email == newCompanyOwner.Email &&
            co.Password == newCompanyOwner.Password)))
            .Returns(new CompanyOwner());

        var result = _companyOwnerService.Create(newCompanyOwner);

        result.AccountState.Should().Be(false);
    }

    [TestMethod]
    public void Create_WithNoCompanyAssociated_ShouldCreateSuccesfully()
    {
        var newCompanyOwner = new CreateUserArgs(
            "CompanyOwnerName",
            "CompanyOwnerSurname",
            "CompanyOwnerEmail@gmail.com",
            "CompanyOwnerPassword123!");

        var companyOwnerRoleId = Guid.NewGuid();

        _companyOwnerRepository
            .Setup(act => act.GetCompanyOwnerRoleId())
            .Returns(companyOwnerRoleId);

        _companyOwnerRepository
            .Setup(act => act.GetRoleById(companyOwnerRoleId))
            .Returns(new Role { RoleName = "company-owner" });

        _companyOwnerRepository
            .Setup(act => act.Exists(co => co.Email == newCompanyOwner.Email))
            .Returns(false);

        _companyOwnerRepository
            .Setup(act => act.Add(It.Is<CompanyOwner>(co =>
            co.Name == newCompanyOwner.Name &&
            co.Surname == newCompanyOwner.Surname &&
            co.Email == newCompanyOwner.Email &&
            co.Password == newCompanyOwner.Password)))
            .Returns(new CompanyOwner());

        var result = _companyOwnerService.Create(newCompanyOwner);

        result.AssociatedCompany.Should().Be(null);
    }
    #endregion
    #endregion

    [TestMethod]
    public void GetById_WithExistentId_ShouldReturnUser()
    {
        var userId = Guid.NewGuid();

        var expectedUser = new CompanyOwner
        {
            Id = userId,
            Name = "Name",
            Surname = "Surname",
            Email = "mail@gmail.com",
            Password = "Password123",
        };

        _companyOwnerRepository
            .Setup(act => act.Exists(u => u.Id == userId))
            .Returns(true);

        _companyOwnerRepository
            .Setup(act => act.GetById(userId))
            .Returns(expectedUser);

        var user = _companyOwnerService.GetById(userId);

        user.Should().BeEquivalentTo(expectedUser);
    }

    [TestMethod]
    public void GetById_WithNonExistentId_ShouldThrowKeyNotFoundException()
    {
        var nonExistentUserId = Guid.NewGuid();

        _companyOwnerRepository
            .Setup(act => act.Exists(u => u.Id == nonExistentUserId))
            .Returns(false);

        var act = () => _companyOwnerService.GetById(nonExistentUserId);

        act.Should().Throw<KeyNotFoundException>().WithMessage("User does not exist");
    }

    [TestMethod]
    public void GiveHomeOwnerRoleToCompanyOwner_WithExistentCompanyOwner_ShouldReturnUpdatedUser()
    {
        var userId = Guid.NewGuid();
        var companyOwnerHomeOwnerRoleId = Guid.NewGuid();
        var creationDate = DateTime.Now;

        var userToBeUpdated = new CompanyOwner
        {
            Id = userId,
            Name = "Name",
            Surname = "Surname",
            Email = "correo@gmail.com",
            Password = "Password123",
            CreationDate = creationDate
        };

        var userResult = new CompanyOwner
        {
            Id = userId,
            Name = "Name",
            Surname = "Surname",
            Email = "correo@gmail.com",
            Password = "Password123",
            RoleId = companyOwnerHomeOwnerRoleId,
            Role = new Role { Id = companyOwnerHomeOwnerRoleId, RoleName = "company-owner-home-owner" },
            CreationDate = creationDate
        };

        _companyOwnerRepository
            .Setup(act => act.Exists(u => u.Id == userId))
            .Returns(true);

        _companyOwnerRepository
            .Setup(act => act.GetById(userId))
            .Returns(userToBeUpdated);

        _companyOwnerRepository
            .Setup(act => act.GetCompanyOwnerHomeOwnerRoleId())
            .Returns(companyOwnerHomeOwnerRoleId);

        _companyOwnerRepository
            .Setup(act => act.GetRoleById(companyOwnerHomeOwnerRoleId))
            .Returns(new Role { Id = companyOwnerHomeOwnerRoleId, RoleName = "company-owner-home-owner" });

        _companyOwnerRepository
            .Setup(act => act.Update(userToBeUpdated))
            .Returns(userResult);

        var resultantUser = _companyOwnerService.GiveHomeOwnerRoleToCompanyOwner(userId);

        resultantUser.Should().BeEquivalentTo(userResult);
    }
}
