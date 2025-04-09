using Domain;
using DTOs;
using FluentAssertions;
using IBusinessLogic;
using IDataAccess;
using ModeloValidador.Abstracciones;
using Moq;
using PaginationAndFilters.Models;

namespace BusinessLogic.Test;

[TestClass]
public class CompanyServiceTest
{
    private CompanyService _companyService = null!;
    private Mock<ICompanyRepository> _companyRepository = null!;
    private Mock<ISessionService> _sessionService = null!;
    private Mock<ICompanyOwnerService> _companyOwnerService = null!;
    private Mock<IAssemblyLoadingService<IModeloValidador>> _assemblyLoadingService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _companyRepository = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        _companyOwnerService = new Mock<ICompanyOwnerService>(MockBehavior.Strict);
        _assemblyLoadingService = new Mock<IAssemblyLoadingService<IModeloValidador>>(MockBehavior.Strict);
        _companyService = new CompanyService(_companyRepository.Object, _assemblyLoadingService.Object, _sessionService.Object, _companyOwnerService.Object);
    }

    #region Create
    #region Error
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithEmptyOrNullCompanyName_ShouldThrowArgumentNullException(string companyName)
    {
        var act = () => new CreateCompanyArgs(companyName, "LogotypeRoute", "RUTNumber", "ModelValidatorId");

        act.Should().Throw<ArgumentNullException>("Company data cannot be empty");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithEmptyOrNullLogotype_ShouldThrowArgumentNullException(string logotype)
    {
        var act = () => new CreateCompanyArgs("CompanyName", logotype, "RUTNumber", "ModelValidatorId");

        act.Should().Throw<ArgumentNullException>("Company data cannot be empty");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void Create_WithEmptyOrNullRut_ShouldThrowArgumentNullException(string rut)
    {
        var act = () => new CreateCompanyArgs("CompanyName", "Logotype", rut, "ModelValidatorId");

        act.Should().Throw<ArgumentNullException>("Company data cannot be empty");
    }

    [TestMethod]
    [DataRow("00000000-0000-0000-0000-000000000000")]
    [DataRow("Not a Guid")]
    public void Create_WithEmptyOrInvalidModelValidatorId_ShouldThrowFormatException(string modelValidatorId)
    {
        var act = () => new CreateCompanyArgs(
            "CompanyName",
            "LogotypeRoute",
            "Rut",
            modelValidatorId);

        act.Should().Throw<FormatException>("Invalid device model validator format");
    }

    [TestMethod]
    public void Create_WithAlreadyExistentRut_ShouldThrowCompanyServiceException()
    {
        // Arrange
        var newCompany = new CreateCompanyArgs(
            "CompanyName",
            "LogotypeRoute",
            "RUTNumberAlreadyRegistered",
            Guid.NewGuid().ToString());

        _companyRepository
            .Setup(act => act.Exists(comp => comp.Rut == newCompany.Rut))
            .Returns(true);

        // Act
        var act = () => _companyService.Create(newCompany, It.IsAny<string>());

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("A company with the entered Rut has already been registered");
        _companyRepository.VerifyAll();
    }

    [TestMethod]
    public void Create_WithNonExistentModelValidationImplementation_ShouldThrowCompanyServiceException()
    {
        // Arrange
        var newCompany = new CreateCompanyArgs(
            "CompanyName",
            "LogotypeRoute",
            "RUTNumber",
            Guid.NewGuid().ToString());

        _companyRepository
            .Setup(act => act.Exists(comp => comp.Rut == newCompany.Rut))
            .Returns(false);

        _assemblyLoadingService
            .Setup(act => act.GetImplementationById(It.IsAny<Guid>()))
            .Returns((IModeloValidador)null);

        // Act
        var act = () => _companyService.Create(newCompany, It.IsAny<string>());

        // Assert
        act.Should().Throw<KeyNotFoundException>().WithMessage("Model validation implementation not found");
        _companyRepository.VerifyAll();
        _assemblyLoadingService.VerifyAll();
    }
    #endregion

    #region Success
    [TestMethod]
    public void Create_WithCorrectData_ShouldCreateSuccesfullyAndHaveCorrectData()
    {
        var modelValidatorId = Guid.NewGuid().ToString();
        var sessionToken = "token";
        var userId = Guid.NewGuid();

        var modelValidator = new Mock<IModeloValidador>(MockBehavior.Strict);

        // Arrange
        var newCompany = new CreateCompanyArgs(
            "CompanyName",
            "LogotypeRoute",
            "RUTNumber",
            modelValidatorId);

        var loggedUser = new User
        {
            Id = userId,
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password"
        };

        var companyOwner = new CompanyOwner
        {
            Id = userId,
            Name = "name",
            Surname = "surname",
            Email = "email",
            Password = "password",
            AssociatedCompany = null
        };

        _companyRepository
            .Setup(act => act.Add(It.Is<Company>(c =>
            c.CompanyName == newCompany.CompanyName &&
            c.Logotype == newCompany.Logotype &&
            c.Rut == newCompany.Rut &&
            c.DeviceModelValidatorId == Guid.Parse(newCompany.DeviceModelValidationId))))
            .Returns(new Company());

        _companyRepository
            .Setup(act => act.Exists(comp => comp.Rut == newCompany.Rut))
            .Returns(false);

        _assemblyLoadingService
            .Setup(act => act.GetImplementationById(It.IsAny<Guid>()))
            .Returns(modelValidator.Object);

        _sessionService
            .Setup(act => act.GetUserByToken("token"))
            .Returns(loggedUser);

        _companyOwnerService
            .Setup(act => act.GetById(userId))
            .Returns(companyOwner);

        // Act
        var result = _companyService.Create(newCompany, sessionToken);

        // Assert
        result.Id.Should().NotBeEmpty();
        Guid.TryParse(result.Id.ToString(), out var _).Should().BeTrue();

        result.CompanyName.Should().Be(newCompany.CompanyName);
        result.Logotype.Should().Be(newCompany.Logotype);
        result.Rut.Should().Be(newCompany.Rut);
        result.DeviceModelValidatorId.Should().Be(Guid.Parse(newCompany.DeviceModelValidationId));
    }
    #endregion
    #endregion

    #region List
    [TestMethod]
    public void GetAllCompanies_WithZeroCompanies_ShouldReturnEmptyList()
    {
        var query = new CompanyFilterArgs();

        _companyRepository
            .Setup(act => act.GetAll(query))
            .Returns([]);

        var companyList = _companyService.GetAllCompanies(query);

        companyList.Count.Should().Be(0);
    }

    [TestMethod]
    public void GetAllCompanies_WithOneCompany_ShouldReturnListWithCreatedCompany()
    {
        var query = new CompanyFilterArgs();

        _companyRepository
            .Setup(act => act.GetAll(query))
            .Returns(
            [new Company
            {
                CompanyName = "CompanyName",
                Logotype = "Logo",
                Rut = "Rut",
                CompanyOwner = new CompanyOwner
                {
                    Name = "OwnerName",
                    Surname = "OwnerSurname",
                    Email = "OwnerEmail",
                    Password = "OwnerPassword"
                }
            }

            ]);

        var companyList = _companyService.GetAllCompanies(query);

        companyList.Count.Should().Be(1);
        companyList[0].CompanyName.Should().Be("CompanyName");
        companyList[0].OwnerFullName.Should().Be("OwnerName OwnerSurname");
        companyList[0].OwnerEmail.Should().Be("OwnerEmail");
        companyList[0].CompanyRut.Should().Be("Rut");
    }

    #endregion
    [TestMethod]
    public void GetCompanyByOwnerId_WithExistentOwner_ShouldReturnCompany()
    {
        var ownerId = Guid.NewGuid();

        var resultantCompany = new Company
        {
            CompanyName = "CompanyName",
            Logotype = "LogotypeRoute",
            Rut = "RUTNumber"
        };

        _companyRepository
            .Setup(act => act.GetCompanyByOwnerId(ownerId))
            .Returns(resultantCompany);

        var company = _companyService.GetCompanyByOwnerId(ownerId);

        company.Should().BeEquivalentTo(resultantCompany);
    }

    [TestMethod]
    public void GetCompanyByOwnerId_WithNonExistentOwner_ShouldThrowException()
    {
        var ownerId = Guid.NewGuid();

        _companyRepository
            .Setup(act => act.GetCompanyByOwnerId(ownerId))
            .Returns((Company?)null);

        var act = () => _companyService.GetCompanyByOwnerId(ownerId);

        act.Should().Throw<KeyNotFoundException>("Logged user does not have an associated company");
    }
}
