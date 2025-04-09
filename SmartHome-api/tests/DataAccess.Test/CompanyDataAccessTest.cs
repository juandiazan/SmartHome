using DataAccess.DBContext;
using Domain;
using FluentAssertions;
using IDataAccess;
using PaginationAndFilters.Models;

namespace DataAccess.Test;

[TestClass]
public class CompanyDataAccessTest
{
    private readonly SmartHomeDBContext _dbContext = DbContextBuilder.BuildTestDbContext();
    private readonly ICompanyRepository _repository;

    public CompanyDataAccessTest()
    {
        _repository = new CompanyRepository(_dbContext);
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
    public void Create_WithCorrectData_ShouldBeAddedToDatabase()
    {
        var newCompany = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = "Rut",
            DeviceModelValidatorId = Guid.NewGuid()
        };

        _repository.Add(newCompany);

        using var otherDbContext = DbContextBuilder.BuildTestDbContext();

        var companies = otherDbContext.Companies.ToList();

        companies.Count.Should().Be(1);
        companies[0].CompanyName.Should().Be("CompanyName");
        companies[0].DeviceModelValidatorId.Should().Be(newCompany.DeviceModelValidatorId);
    }

    [TestMethod]
    public void GetAll_WithOneElementInDatabase_ShouldListOne()
    {
        var expectedEntity = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = "Rut"
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(expectedEntity);
        context.SaveChanges();

        var entitiesSaved = _repository.GetAll(new CompanyFilterArgs());

        entitiesSaved.Count.Should().Be(1);
        entitiesSaved[0].CompanyName.Should().Be(expectedEntity.CompanyName);
        entitiesSaved[0].Rut.Should().Be(expectedEntity.Rut);
    }

    [TestMethod]
    public void GetAllFilter_ByCompanyNameWithTwoElementsInDatabase_ShouldListOne()
    {
        var expectedEntity = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "Rut"
        };

        var unexpectedEntity = new Company
        {
            CompanyName = "DifferentCompanyName",
            Logotype = "Logotype",
            Rut = "DifferentRut"
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(expectedEntity);
        context.Add(unexpectedEntity);
        context.SaveChanges();

        var query = new CompanyFilterArgs(null, null, expectedEntity.CompanyName, null);
        var entitiesSaved = _repository.GetAll(query);

        entitiesSaved.Count.Should().Be(1);
        entitiesSaved[0].CompanyName.Should().Be("ExpectedCompanyName");
    }

    [TestMethod]
    public void GetAllFilter_ByOwnerFullNameWithTwoElementsInDatabase_ShouldListZero()
    {
        var user = new CompanyOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email",
            Password = "Password",
            RoleId = SmartHomeDBContext.CompanyOwnerRoleId
        };

        var anotherUser = new CompanyOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email2",
            Password = "Password",
            RoleId = SmartHomeDBContext.CompanyOwnerRoleId
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(user);
        context.Add(anotherUser);
        context.SaveChanges();

        var expectedEntity = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "Rut",
            CompanyOwner = user
        };

        var unexpectedEntity = new Company
        {
            CompanyName = "DifferentCompanyName",
            Logotype = "Logotype",
            Rut = "DifferentRut",
            CompanyOwner = anotherUser
        };

        context.Add(expectedEntity);
        context.Add(unexpectedEntity);
        context.SaveChanges();

        var query = new CompanyFilterArgs(null, null, null, "ownerFullName");
        var entitiesSaved = _repository.GetAll(query);

        entitiesSaved.Count.Should().Be(0);
    }

    [TestMethod]
    public void GetAllPaginate_WithTwoElementsAndCurrentPageTwoAndPageSizeOne_ShouldListOneElement()
    {
        var currentPage = 2;
        var pageSize = 1;
        var expectedValue = 1;

        var firstEntity = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = "FirstRut"
        };

        var secondEntity = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = "SecondRut"
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(firstEntity);
        context.Add(secondEntity);
        context.SaveChanges();

        var query = new CompanyFilterArgs(currentPage, pageSize, null, null);
        var entitiesSaved = _repository.GetAll(query);

        entitiesSaved.Count.Should().Be(expectedValue);
    }

    [TestMethod]
    public void GetAllPaginate_WithTwoElementsAndCurrentPageThreeAndPageSizeTwo_ShouldListZeroElements()
    {
        var currentPage = 3;
        var pageSize = 2;
        var expectedValue = 0;

        var firstEntity = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = "FirstRut"
        };

        var secondEntity = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = "SecondRut"
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(firstEntity);
        context.Add(secondEntity);
        context.SaveChanges();

        var query = new CompanyFilterArgs(currentPage, pageSize, null, null);
        var entitiesSaved = _repository.GetAll(query);

        entitiesSaved.Count.Should().Be(expectedValue);
    }

    [TestMethod]
    public void GetAllPaginate_WithTwoElementsCurrentPageOneAndPageSizeTwo_ShouldListTwoElements()
    {
        var currentPage = 1;
        var pageSize = 2;
        var expectedValue = 2;

        var firstEntity = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = "FirstRut"
        };

        var secondEntity = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = "SecondRut"
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(firstEntity);
        context.Add(secondEntity);
        context.SaveChanges();

        var query = new CompanyFilterArgs(currentPage, pageSize, null, null);
        var entitiesSaved = _repository.GetAll(query);

        entitiesSaved.Count.Should().Be(expectedValue);
    }

    [TestMethod]
    public void GetAllPaginateAndFilter_WithCompanyNameAndPageSizeTwoAndCurrentPageTwoAndTwoElements_ShouldListOneElem()
    {
        var currentPage = 2;
        var pageSize = 2;
        var expectedValue = 1;

        var firstEntity = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "FirstRut"
        };

        var secondEntity = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "SecondRut"
        };

        var thirdEntity = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "ThirdRut"
        };

        var fourthEntity = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "Logotype",
            Rut = "FourthRut"
        };

        using var context = DbContextBuilder.BuildTestDbContext();
        context.Add(firstEntity);
        context.Add(secondEntity);
        context.Add(thirdEntity);
        context.Add(fourthEntity);
        context.SaveChanges();

        var query = new CompanyFilterArgs(currentPage, pageSize, "ExpectedCompanyName", null);
        var entitiesSaved = _repository.GetAll(query);

        entitiesSaved.Count.Should().Be(expectedValue);
        entitiesSaved[0].CompanyName.Should().Be("ExpectedCompanyName");
    }

    [TestMethod]
    public void GetCompanyByOwnerId_WithExistentUser_ShouldReturnCompany()
    {
        var company = new Company
        {
            CompanyName = "ExpectedCompanyName",
            Logotype = "Logotype",
            Rut = "Rut"
        };

        var user = new CompanyOwner
        {
            Name = "Name",
            Surname = "Surname",
            Email = "Email",
            Password = "Password",
            RoleId = SmartHomeDBContext.CompanyOwnerRoleId,
            Role = _dbContext.Roles.First(r => r.Id == SmartHomeDBContext.CompanyOwnerRoleId),
            AssociatedCompanyId = company.Id,
            AssociatedCompany = company
        };

        _dbContext.Add(company);
        _dbContext.Add(user);
        _dbContext.SaveChanges();

        var result = _repository.GetCompanyByOwnerId(user.Id);
        result.Should().Be(company);
    }
}
