using DataAccess.DBContext;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PaginationAndFilters.Models;

namespace DataAccess.Test;

[TestClass]
public class UserDataAccessTest
{
    private readonly SmartHomeDBContext _context = DbContextBuilder.BuildTestDbContext();
    private readonly UserRepository _repository;

    public UserDataAccessTest()
    {
        _repository = new UserRepository(_context);
    }

    [TestInitialize]
    public void Initialize()
    {
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Add_WithCorrectData_ShouldBeInDatabaseAndBeAdministrator()
    {
        var user = new User
        {
            Name = "FirstName",
            Surname = "LastName",
            Email = "Email",
            Password = "Password",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        _repository.Add(user);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();
        var users = otherDbContext.Users.Include(u => u.Role).ThenInclude(r => r.Permissions).ToList();

        users.Count.Should().Be(2);
        users[1].Id.Should().Be(user.Id);
        users[1].RoleId.Should().Be(SmartHomeDBContext.AdministratorRoleId);
        users[1].Role.RoleName.Should().Be("administrator");
        _context.Permissions.Any(p => p.Name == "create-administrator").Should().BeTrue();
        _context.Permissions.Any(p => p.Name == "create-companyowner").Should().BeTrue();
        _context.Permissions.Any(p => p.Name == "list-users").Should().BeTrue();
        _context.Permissions.Any(p => p.Name == "list-companies").Should().BeTrue();
        users[1].Role.Permissions.Any(p => p.Name == "create-administrator").Should().BeTrue();
        users[1].Role.Permissions.Any(p => p.Name == "create-companyowner").Should().BeTrue();
        users[1].Role.Permissions.Any(p => p.Name == "list-users").Should().BeTrue();
        users[1].Role.Permissions.Any(p => p.Name == "list-companies").Should().BeTrue();
        _repository.GetUserByEmail(user.Email).Should().NotBeNull();
    }

    [TestMethod]
    public void DeleteById_WithTwoUsers_ShouldBeOneInDatabase()
    {
        var user1 = new User
        {
            Name = "FirstNameOne",
            Surname = "LastNameOne",
            Email = "EmailOne",
            Password = "PasswordOne",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        var user2 = new User
        {
            Name = "FirstNameTwo",
            Surname = "LastNameTwo",
            Email = "EmailTwo",
            Password = "PasswordTwo",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        _repository.Add(user1);
        _repository.Add(user2);
        _context.SaveChanges();

        _repository.DeleteById(user1.Id);

        var result = _context.Users.ToList();

        _repository.Exists(u => u.Id == user2.Id);
        result.Count.Should().Be(2);
        result[1].Should().BeEquivalentTo(user2);
    }

    [TestMethod]
    public void GetAll_WithoutPaginationOrFilters_ShouldReturnCorrectElements()
    {
        var user1 = new User
        {
            Name = "FirstNameOne",
            Surname = "LastNameOne",
            Email = "EmailOne",
            Password = "PasswordOne",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        var user2 = new User
        {
            Name = "FirstNameTwo",
            Surname = "LastNameTwo",
            Email = "EmailTwo",
            Password = "PasswordTwo",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        _repository.Add(user1);
        _repository.Add(user2);

        var result = _repository.GetAll(new UserFilterArgs());

        result.Count.Should().Be(3);
        result[1].Name.Should().Be("FirstNameOne");
        result[1].Surname.Should().Be("LastNameOne");
        result[2].Name.Should().Be("FirstNameTwo");
        result[2].Surname.Should().Be("LastNameTwo");
    }

    [TestMethod]
    public void GetAll_WithPagination_ShouldReturnOneElement()
    {
        var user1 = new User
        {
            Name = "FirstNameOne",
            Surname = "LastNameOne",
            Email = "EmailOne",
            Password = "PasswordOne",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        var user2 = new User
        {
            Name = "FirstNameTwo",
            Surname = "LastNameTwo",
            Email = "EmailTwo",
            Password = "PasswordTwo",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        _repository.Add(user1);
        _repository.Add(user2);

        var filterArgs = new UserFilterArgs(2, 1, null, null);
        var result = _repository.GetAll(filterArgs);

        result.Count.Should().Be(1);
        result[0].Name.Should().Be("FirstNameOne");
        result[0].Surname.Should().Be("LastNameOne");
        result[0].Role.RoleName.Should().Be("administrator");
    }

    [TestMethod]
    public void GetAll_FilterByFullName_ShouldReturnOneElement()
    {
        var user1 = new User
        {
            Name = "FirstNameOne",
            Surname = "LastNameOne",
            Email = "EmailOne",
            Password = "PasswordOne",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        var user2 = new User
        {
            Name = "FirstNameTwo",
            Surname = "LastNameTwo",
            Email = "EmailTwo",
            Password = "PasswordTwo",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        _repository.Add(user1);
        _repository.Add(user2);

        var filterArgs = new UserFilterArgs(null, null, "FirstNameOne LastNameOne", null);
        var result = _repository.GetAll(filterArgs);

        result.Count.Should().Be(1);
        result[0].Name.Should().Be("FirstNameOne");
        result[0].Surname.Should().Be("LastNameOne");
    }

    [TestMethod]
    public void GetAll_FilterByRole_ShouldReturnTwoElementsAndSeedData()
    {
        var user1 = new User
        {
            Name = "FirstNameOne",
            Surname = "LastNameOne",
            Email = "EmailOne",
            Password = "PasswordOne",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        var user2 = new User
        {
            Name = "FirstNameTwo",
            Surname = "LastNameTwo",
            Email = "EmailTwo",
            Password = "PasswordTwo",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        _repository.Add(user1);
        _repository.Add(user2);

        var result = _repository.GetAll(new UserFilterArgs(null, null, null, "administrator"));

        result.Count.Should().Be(3);
        result[1].Name.Should().Be("FirstNameOne");
        result[1].Surname.Should().Be("LastNameOne");
        result[1].Role.RoleName.Should().Be("administrator");
    }

    [TestMethod]
    public void IsPasswordCorrect_ShouldReturnTrue_WhenPasswordIsCorrect()
    {
        var user = new User
        {
            Name = "FirstName",
            Surname = "LastName",
            Email = "Email",
            Password = "CorrectPassword",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        _repository.Add(user);
        _context.SaveChanges();

        var result = _repository.IsPasswordCorrect(user.Email, "CorrectPassword");

        result.Should().BeTrue();
    }

    [TestMethod]
    public void GetById_WithCorrectData_ShouldReturnCorrectUser()
    {
        var user = new User
        {
            Name = "FirstName",
            Surname = "LastName",
            Email = "Email",
            Password = "Password",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        _repository.Add(user);

        var result = _repository.GetUserById(user.Id);

        result.Should().BeEquivalentTo(user);
    }

    [TestMethod]
    public void GetAdminHomeOwnerRoleId_ShouldReturnCorrectRoleId()
    {
        var result = _repository.GetAdminHomeOwnerRoleId();

        result.Should().Be(SmartHomeDBContext.AdministratorHomeOwnerRoleId);
    }

    [TestMethod]
    public void Update_WithCorrectData_ShouldUpdateUser()
    {
        var user = new CompanyOwner
        {
            Name = "FirstName",
            Surname = "LastName",
            Email = "Email",
            Password = "Password",
            RoleId = _repository.GetAdminRoleId(),
            Role = _repository.GetRoleById(_repository.GetAdminRoleId())
        };

        _repository.Add(user);

        user.RoleId = _repository.GetAdminHomeOwnerRoleId();
        user.Role = _repository.GetRoleById(_repository.GetAdminHomeOwnerRoleId());

        _repository.Update(user);

        var result = _repository.GetUserById(user.Id);

        result.RoleId.Should().Be(SmartHomeDBContext.AdministratorHomeOwnerRoleId);
    }
}
