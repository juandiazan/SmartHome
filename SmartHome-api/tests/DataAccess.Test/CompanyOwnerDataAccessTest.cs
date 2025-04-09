using DataAccess.DBContext;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test;

[TestClass]
public class CompanyOwnerDataAccessTest
{
    private readonly SmartHomeDBContext _context = DbContextBuilder.BuildTestDbContext();
    private readonly CompanyOwnerRepository _repository;

    public CompanyOwnerDataAccessTest()
    {
        _repository = new CompanyOwnerRepository(_context);
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
    public void Add_WithCorrectCompanyOwner_ShouldBeOneInDatabaseWithRoleAndPermissions()
    {
        var companyOwner = new CompanyOwner
        {
            Name = "FirstName",
            Surname = "LastName",
            Email = "Email",
            Password = "Password",
            RoleId = _repository.GetCompanyOwnerRoleId(),
            Role = _repository.GetRoleById(_repository.GetCompanyOwnerRoleId())
        };

        _repository.Add(companyOwner);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();
        var companyOwners = otherDbContext.CompanyOwners.Include(u => u.Role).ThenInclude(r => r.Permissions).ToList();

        _repository.Exists(co => co.Id == companyOwner.Id);
        companyOwners.Count.Should().Be(1);
        companyOwners[0].RoleId.Should().Be(SmartHomeDBContext.CompanyOwnerRoleId);
        companyOwners[0].Role.RoleName.Should().Be("company-owner");
        companyOwners[0].Role.Permissions.Any(p => p.Name == "create-company").Should().BeTrue();
        companyOwners[0].Role.Permissions.Any(p => p.Name == "create-camera").Should().BeTrue();
        companyOwners[0].Role.Permissions.Any(p => p.Name == "create-sensor").Should().BeTrue();
    }

    [TestMethod]
    public void AssociateCompany_WithCorrectData_ShouldAssociateCorrectly()
    {
        var companyOwner = new CompanyOwner
        {
            Name = "FirstName",
            Surname = "LastName",
            Email = "Email",
            Password = "Password",
            RoleId = _repository.GetCompanyOwnerRoleId(),
            Role = _repository.GetRoleById(_repository.GetCompanyOwnerRoleId())
        };

        var company = new Company
        {
            CompanyName = "Company",
            Logotype = "Logotype",
            Rut = "Rut"
        };

        _repository.Add(companyOwner);
        _context.Companies.Add(company);
        _context.SaveChanges();

        _repository.AssociateCompany(company.Id, companyOwner.Id);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();
        var companyOwners = otherDbContext.CompanyOwners.Include(c => c.AssociatedCompany).ToList();

        companyOwners[0].AssociatedCompanyId.Should().Be(company.Id);
        companyOwners[0].AssociatedCompany.CompanyName.Should().Be(company.CompanyName);
        companyOwners[0].AssociatedCompany.Rut.Should().Be(company.Rut);
        companyOwners[0].AccountState.Should().BeTrue();
    }

    [TestMethod]
    public void GetById_WithCorrectData_ShouldReturnCorrectUser()
    {
        var user = new CompanyOwner
        {
            Name = "FirstName",
            Surname = "LastName",
            Email = "Email",
            Password = "Password",
            RoleId = _repository.GetCompanyOwnerRoleId(),
            Role = _repository.GetRoleById(_repository.GetCompanyOwnerRoleId())
        };

        _repository.Add(user);

        var result = _repository.GetById(user.Id);

        result.Should().BeEquivalentTo(user);
    }

    [TestMethod]
    public void GetAdminHomeOwnerRoleId_ShouldReturnCorrectRoleId()
    {
        var result = _repository.GetCompanyOwnerHomeOwnerRoleId();

        result.Should().Be(SmartHomeDBContext.CompanyOwnerHomeOwnerRoleId);
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
            RoleId = _repository.GetCompanyOwnerRoleId(),
            Role = _repository.GetRoleById(_repository.GetCompanyOwnerRoleId())
        };

        _repository.Add(user);

        user.RoleId = _repository.GetCompanyOwnerHomeOwnerRoleId();
        user.Role = _repository.GetRoleById(_repository.GetCompanyOwnerHomeOwnerRoleId());

        _repository.Update(user);

        var result = _repository.GetById(user.Id);

        result.RoleId.Should().Be(SmartHomeDBContext.CompanyOwnerHomeOwnerRoleId);
    }
}
