using System.Linq.Expressions;
using Domain;
using DTOs;
using FluentAssertions;
using IDataAccess;
using Moq;
using PaginationAndFilters.Models;

namespace BusinessLogic.Test;

[TestClass]
public class UserServiceTest
{
    private Mock<IAdministratorRepository> _userRepository = null!;
    private Mock<IUserHasSessionActive> _sessionService = null!;

    private UserService _userService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _userRepository = new Mock<IAdministratorRepository>(MockBehavior.Strict);
        _sessionService = new Mock<IUserHasSessionActive>(MockBehavior.Strict);
        _userService = new UserService(_userRepository.Object, _sessionService.Object);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithNullOrEmptyName_ShouldThrowException(string userName)
    {
        var act = () => new CreateUserArgs(userName, "Surname", "user@email.com", "Password");

        act.Should().Throw<ArgumentNullException>("Administrator name cannot be null or empty");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithNullOrEmptySurname_ShouldThrowException(string adminSurname)
    {
        var act = () => new CreateUserArgs("UserName", adminSurname, "user@email.com", "Password");

        act.Should().Throw<ArgumentNullException>("Administrator surname cannot be null or empty");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithNullOrEmptyEmail_ShouldThrowException(string adminEmail)
    {
        var act = () => new CreateUserArgs("UserName", "Surname", adminEmail, "Password");

        act.Should().Throw<ArgumentNullException>("Administrator email cannot be null or empty");
    }

    [TestMethod]
    public void Create_EmailFormatIsInvalid_ShouldThrowException()
    {
        var act = () => new CreateUserArgs("UserName", "Surname", "NotFormattedEmail", "Password");

        act.Should().Throw<FormatException>().WithMessage("Administrator email invalid format");
    }

    [TestMethod]
    public void Create_WithDuplicatedEmail_ShouldThrowException()
    {
        var newCompanyOwner = new CreateUserArgs(
            "CompanyOwnerName",
            "CompanyOwnerSurname",
            "CompanyOwnerEmail@gmail.com",
            "CompanyOwnerPassword123!");

        _userRepository
            .Setup(act => act.Exists(co => co.Email == newCompanyOwner.Email))
        .Returns(true);

        var act = () => _userService.Create(newCompanyOwner);

        act.Should().Throw<InvalidOperationException>("A user with the entered email has already been registered");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_PasswordIsNullOrEmpty_ShouldThrowException(string adminPasword)
    {
        var act = () => new CreateUserArgs("UserName", "Surname", "user@gmail.com", adminPasword);

        act.Should().Throw<ArgumentNullException>("Password cannot be null");
    }

    [TestMethod]
    public void Create_PasswordLengthIsLessThanSix_ShouldThrowException()
    {
        var adminPasword = "six";

        var act = () => new CreateUserArgs("UserName", "Surname", "user@gmail.com", adminPasword);

        act.Should().Throw<FormatException>().WithMessage("Administrator password length should be at least six");
    }

    [TestMethod]
    public void Create_HasNotEspecialCharacters_ShouldThrowException()
    {
        var userPassword = "PasswordWithNotEspecialCharactersInIt";

        var act = () => new CreateUserArgs("UserName", "Surname", "user@gmail.com", userPassword);

        act.Should().Throw<FormatException>().WithMessage("The password must have at least one especial character");
    }

    [TestMethod]
    public void Create_WithValidData_ShouldReturnUser()
    {
        // Arrange
        var creationDate = DateTime.Today;

        var newUser = new CreateUserArgs(
            "UserName",
            "UserSurname",
            "user@gmail.com",
            "password!123");

        var adminRoleId = Guid.NewGuid();

        _userRepository
            .Setup(act => act.Exists(u => u.Email == newUser.Email))
            .Returns(false);

        _userRepository
            .Setup(act => act.GetAdminRoleId())
            .Returns(adminRoleId);

        _userRepository
            .Setup(act => act.GetRoleById(adminRoleId))
            .Returns(new Role { RoleName = "administrator" });

        _userRepository
            .Setup(act => act.Add(It.Is<User>(u =>
            u.Email == newUser.Email &&
            u.Surname == newUser.Surname &&
            u.Name == newUser.Name &&
            u.Password == newUser.Password &&
            u.RoleId == adminRoleId)))
            .Returns(It.Is<User>(u => u.Email == newUser.Email));

        // Act
        var createdUser = _userService.Create(newUser);

        // Assert
        createdUser.Should().NotBeNull();
        createdUser.Name.Should().Be("UserName");
        createdUser.Surname.Should().Be("UserSurname");
        createdUser.Email.Should().Be("user@gmail.com");
        createdUser.Password.Should().Be("password!123");
        createdUser.CreationDate.Date.Should().Be(creationDate);
        createdUser.RoleId.Should().Be(adminRoleId);
        createdUser.Role.RoleName.Should().Be("administrator");
    }

    [TestMethod]
    public void GetAll_WithTwoUsers_ShouldReturnTwoUsers()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var creationDate = DateTime.Now;

        var userArgs1 = new GetAllUserArgs(
            id1.ToString(),
            "Name",
            "Surname",
            "Name Surname",
            "company-owner",
            creationDate.ToString());

        var userArgs2 = new GetAllUserArgs(
            id2.ToString(),
            "Name",
            "Surname",
            "Name Surname",
            "home-owner",
            creationDate.ToString());

        var expectedList = new List<GetAllUserArgs> { userArgs1, userArgs2 };

        var user1 = new User
        {
            Id = id1,
            Name = "Name",
            Surname = "Surname",
            Email = "mail",
            Password = "pass",
            Role = new Role { RoleName = "company-owner" }
        };

        var user2 = new User
        {
            Id = id2,
            Name = "Name",
            Surname = "Surname",
            Email = "mail2",
            Password = "pass",
            Role = new Role { RoleName = "home-owner" }
        };

        var query = new UserFilterArgs();
        _userRepository
            .Setup(act => act.GetAll(query))
            .Returns([user1, user2]);

        var result = _userService.GetAll(query);

        result.Should().BeEquivalentTo(expectedList);
    }

    [TestMethod]
    [DataRow(0, 2)]
    [DataRow(-1, 2)]
    [DataRow(2, 0)]
    [DataRow(2, -1)]
    public void GetAll_WithNegativeOrZeroForPagination_ShouldThrowException(int page, int pageSize)
    {
        var query = new UserFilterArgs(page, pageSize, null, null);
        var act = () => _userService.GetAll(query);

        act.Should().Throw<FormatException>("Current page and page size cannot be negative or zero");
    }

    [TestMethod]
    public void DeleteById_WithCorrectId_ShouldDelete()
    {
        var user1 = new User
        {
            Name = "Name",
            Surname = "Surname",
            Email = "user@gmail.com",
            Password = "Password123!",
        };

        _userRepository
        .Setup(act => act.Exists(It.IsAny<Expression<Func<User, bool>>>()))
        .Returns(true);

        _userRepository
            .Setup(act => act.DeleteById(user1.Id))
            .Returns(user1);

        _sessionService
            .Setup(act => act.HasActiveSessionById(user1.Id))
            .Returns(false);

        var result = _userService.DeleteById(user1.Id);

        result.Should().BeEquivalentTo(user1);
    }

    [TestMethod]
    public void DeleteById_WithNonExistentUserId_ShouldThrowException()
    {
        var user = Guid.NewGuid();

        _userRepository
            .Setup(act => act.Exists(u => u.Id == user))
            .Returns(false);

        var act = () => _userService.DeleteById(user);

        act.Should().Throw<KeyNotFoundException>("User to delete does not exist");
    }

    [TestMethod]
    public void DeleteById_WithActiveSession_ShouldThrowException()
    {
        var user = Guid.NewGuid();

        _userRepository
            .Setup(act => act.Exists(u => u.Id == user))
            .Returns(true);

        _sessionService
            .Setup(act => act.HasActiveSessionById(user))
            .Returns(true);

        var act = () => _userService.DeleteById(user);

        act.Should().Throw<InvalidOperationException>("User cannot be deleted because of an active session");
    }

    [TestMethod]
    public void GetUserByEmail_WithNonExistentUser_ShouldThrowException()
    {
        var nonExistentUser = "user@gmail.com";

        _userRepository
            .Setup(act => act.Exists(u => u.Email == nonExistentUser))
            .Returns(false);

        var act = () => _userService.GetUserByEmail(nonExistentUser);

        act.Should().Throw<KeyNotFoundException>("User with entered email does not exist");
    }

    [TestMethod]
    public void GetById_WithExistentId_ShouldReturnUser()
    {
        var userId = Guid.NewGuid();

        var expectedUser = new User
        {
            Id = userId,
            Name = "Name",
            Surname = "Surname",
            Email = "mail@gmail.com",
            Password = "Password123",
        };

        _userRepository
            .Setup(act => act.Exists(u => u.Id == userId))
            .Returns(true);

        _userRepository
            .Setup(act => act.GetUserById(userId))
            .Returns(expectedUser);

        var user = _userService.GetUserById(userId);

        user.Should().BeEquivalentTo(expectedUser);
    }

    [TestMethod]
    public void GetById_WithNonExistentId_ShouldThrowKeyNotFoundException()
    {
        var nonExistentUserId = Guid.NewGuid();

        _userRepository
            .Setup(act => act.Exists(u => u.Id == nonExistentUserId))
            .Returns(false);

        var act = () => _userService.GetUserById(nonExistentUserId);

        act.Should().Throw<KeyNotFoundException>().WithMessage("User does not exist");
    }

    [TestMethod]
    public void GiveHomeOwnerRoleToAdmin_WithExistentUser_ShouldReturnUpdatedUser()
    {
        var userId = Guid.NewGuid();
        var adminHomeOwnerRoleId = Guid.NewGuid();
        var creationDate = DateTime.Now;

        var userToBeUpdated = new User
        {
            Id = userId,
            Name = "Name",
            Surname = "Surname",
            Email = "correo@gmail.com",
            Password = "Password123",
            CreationDate = creationDate
        };

        var userResult = new User
        {
            Id = userId,
            Name = "Name",
            Surname = "Surname",
            Email = "correo@gmail.com",
            Password = "Password123",
            RoleId = adminHomeOwnerRoleId,
            Role = new Role { Id = adminHomeOwnerRoleId, RoleName = "admin-home-owner" },
            CreationDate = creationDate
        };

        _userRepository
            .Setup(act => act.Exists(u => u.Id == userId))
            .Returns(true);

        _userRepository
            .Setup(act => act.GetUserById(userId))
            .Returns(userToBeUpdated);

        _userRepository
            .Setup(act => act.GetAdminHomeOwnerRoleId())
            .Returns(adminHomeOwnerRoleId);

        _userRepository
            .Setup(act => act.GetRoleById(adminHomeOwnerRoleId))
            .Returns(new Role { Id = adminHomeOwnerRoleId, RoleName = "admin-home-owner" });

        _userRepository
            .Setup(act => act.Update(userToBeUpdated))
            .Returns(userResult);

        var resultantUser = _userService.UpdateRoleOfAdministratorToHomeOwner(userId);

        resultantUser.Should().BeEquivalentTo(userResult);
    }
}
