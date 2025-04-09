using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using Moq;

namespace BusinessLogic.Test;

[TestClass]
public class HomeOwnerServiceTest
{
    private Mock<IHomeOwnerRepository> _homeOwnerRepository = null!;
    private HomeOwnerService _homeOwnerService = null!;
    private Mock<ISessionService> _sessionService = null!;
    private Mock<IHomeService> _homeService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _homeOwnerRepository = new Mock<IHomeOwnerRepository>(MockBehavior.Strict);
        _sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        _homeService = new Mock<IHomeService>(MockBehavior.Strict);
        _homeOwnerService = new HomeOwnerService(_homeOwnerRepository.Object, _sessionService.Object, _homeService.Object);
    }

    #region Create
    #region Error
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithEmptyOrNullProfilePicture_ShouldThrowHomeOwnerServiceException(string profilePicture)
    {
        var act = () => new CreateHomeOwnerArgs("HomeOwnerName", "homeOwnerSurname", profilePicture, "Email@gmail.com", "Password123!");

        act.Should().Throw<ArgumentNullException>("Profile picture cannot be null.");
    }

    [TestMethod]
    public void Create_WithAlreadyExistentEmail_ShouldThrowHomeOwnerServiceException()
    {
        var newHomeOwner = new CreateHomeOwnerArgs(
            "HomeOwnerName",
            "homeOwnerSurname",
            "profilePicture",
            "Email@gmail.com",
            "Password123!");

        _homeOwnerRepository
            .Setup(act => act.Exists(co => co.Email == newHomeOwner.Email))
        .Returns(true);

        var act = () => _homeOwnerService.Create(newHomeOwner);

        act.Should().Throw<InvalidOperationException>().WithMessage("A user with the entered email has already been registered");
    }
    #endregion
    #region Success
    [TestMethod]
    public void Create_WithCorrectData_ShouldCreateCorrectly()
    {
        var newHomeOwner = new CreateHomeOwnerArgs(
            "HomeOwnerName",
            "homeOwnerSurname",
            "profilePicture",
            "Email@gmail.com",
            "Password123!");

        var homeOwnerRoleId = Guid.NewGuid();

        _homeOwnerRepository
            .Setup(act => act.GetHomeOwnerRoleId())
            .Returns(homeOwnerRoleId);

        _homeOwnerRepository
            .Setup(act => act.GetRoleById(homeOwnerRoleId))
            .Returns(new Role { RoleName = "home-owner" });

        _homeOwnerRepository
            .Setup(act => act.Exists(co => co.Email == newHomeOwner.Email))
        .Returns(false);

        _homeOwnerRepository
            .Setup(act => act.Add(It.Is<HomeOwner>(ho =>
            ho.Name == newHomeOwner.Name &&
            ho.Surname == newHomeOwner.Surname &&
            ho.ProfilePicture == newHomeOwner.ProfilePicture &&
            ho.Email == newHomeOwner.Email &&
            ho.Password == newHomeOwner.Password &&
            ho.RoleId == homeOwnerRoleId)))
            .Returns(It.Is<HomeOwner>(ho => ho.Email == newHomeOwner.Email));

        var result = _homeOwnerService.Create(newHomeOwner);

        result.Name.Should().Be(newHomeOwner.Name);
        result.Surname.Should().Be(newHomeOwner.Surname);
        result.ProfilePicture.Should().Be(newHomeOwner.ProfilePicture);
        result.Email.Should().Be(newHomeOwner.Email);
        result.Password.Should().Be(newHomeOwner.Password);
        result.CreationDate.Date.Should().Be(DateTime.Today);
        result.RoleId.Should().Be(homeOwnerRoleId);
        result.Role.RoleName.Should().Be("home-owner");
    }
    #endregion
    #endregion

    #region GetHomeOwnerOwnedHomeId

    [TestMethod]
    public void GetHomeOwnerOwnedHomeId_WithCorrectData_ShouldReturnCorrectHomeId()
    {
        var homeOwnerId = Guid.NewGuid();
        var token = "validToken";
        var expectedHomeId = Guid.NewGuid();

        _sessionService
            .Setup(s => s.GetUserByToken(token))
            .Returns(new HomeOwner { Id = homeOwnerId });

        _homeService
            .Setup(h => h.GetHomeByHomeOwnerId(homeOwnerId))
            .Returns(new Home { Id = expectedHomeId });

        var result = _homeOwnerService.GetHomeOwnerOwnedHomeId(token);

        result.Should().Be(expectedHomeId.ToString());
    }

    #endregion
}
