using DataAccess.DBContext;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test;

[TestClass]
public class SessionDataAccessTest
{
    private readonly SmartHomeDBContext _dbContext = DbContextBuilder.BuildTestDbContext();
    private readonly SessionRepository _repository;

    public SessionDataAccessTest()
    {
        _repository = new SessionRepository(_dbContext);
    }

    [TestInitialize]
    public void Initialize()
    {
        _dbContext.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Add_WithCorrectData_ShouldBeInDatabase()
    {
        var user = new User
        {
            Name = "FirstName",
            Surname = "LastName",
            Email = "Email",
            Password = "Password",
            RoleId = SmartHomeDBContext.AdministratorRoleId
        };

        _dbContext.Add(user);
        _dbContext.SaveChanges();

        var session = new Session
        {
            Id = Guid.NewGuid(),
            SessionToken = "token",
            User = user
        };

        _repository.Add(session);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();

        var sessions = otherDbContext.Sessions.Include(u => u.User).ToList();

        sessions.Count.Should().Be(1);
        sessions[0].User.Should().NotBeNull();
        _repository.HasActiveSessionByEmail(user.Email).Should().BeTrue();
        _repository.IsAuthenticated("token").Should().BeTrue();
        _repository.GetUserByToken("token").Should().NotBeNull();
    }

    [TestMethod]
    public void GetSessionTokenByEmail_WithCorrectEmail_ShouldReturnToken()
    {
        var user = new User
        {
            Name = "FirstName",
            Surname = "LastName",
            Email = "Email",
            Password = "Password",
            RoleId = SmartHomeDBContext.AdministratorRoleId
        };

        _dbContext.Add(user);
        _dbContext.SaveChanges();

        var session = new Session
        {
            Id = Guid.NewGuid(),
            SessionToken = "token",
            User = user
        };

        _repository.Add(session);

        var token = _repository.GetSessionTokenByEmail(user.Email);

        token.Should().Be("token");
    }

    [TestMethod]
    public void HasActiveSessionById_WithCorrectId_ShouldReturnTrue()
    {
        var user = new User
        {
            Name = "FirstName",
            Surname = "LastName",
            Email = "Email",
            Password = "Password",
            RoleId = SmartHomeDBContext.AdministratorRoleId
        };

        _dbContext.Add(user);
        _dbContext.SaveChanges();

        var session = new Session
        {
            Id = Guid.NewGuid(),
            SessionToken = "token",
            User = user
        };

        _repository.Add(session);
        _dbContext.SaveChanges();

        _repository.HasActiveSessionById(user.Id).Should().BeTrue();
    }
}
