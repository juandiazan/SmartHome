using DataAccess.DBContext;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test;

[TestClass]
public class HomeOwnerDataAccessTest
{
    private readonly SmartHomeDBContext _context = DbContextBuilder.BuildTestDbContext();
    private readonly HomeOwnerRepository _repository;

    public HomeOwnerDataAccessTest()
    {
        _repository = new HomeOwnerRepository(_context);
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
    public void Add_WithCorrectHomeOwner_ShouldBeOneInDatabaseWithRoleAndPermissions()
    {
        var homeOwner = new HomeOwner
        {
            Name = "FirstName",
            Surname = "LastName",
            ProfilePicture = "ProfilePicture",
            Email = "Email",
            Password = "Password",
            RoleId = _repository.GetHomeOwnerRoleId(),
            Role = _repository.GetRoleById(_repository.GetHomeOwnerRoleId())
        };

        _repository.Add(homeOwner);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();
        var homeOwners = otherDbContext.HomeOwners.Include(u => u.Role).ThenInclude(r => r.Permissions).ToList();

        _repository.Exists(ho => ho.Id == homeOwner.Id);
        homeOwners.Count.Should().Be(1);
        homeOwners[0].RoleId.Should().Be(SmartHomeDBContext.HomeOwnerRoleId);
        homeOwners[0].Role.RoleName.Should().Be("home-owner");
        homeOwners[0].Role.Permissions.Any(p => p.Name == "create-home").Should().BeTrue();
        homeOwners[0].Role.Permissions.Any(p => p.Name == "add-member-to-home").Should().BeTrue();
        homeOwners[0].Role.Permissions.Any(p => p.Name == "add-permissions-to-member").Should().BeTrue();
    }
}
