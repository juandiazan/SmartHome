using Domain;
using DTOs;
using IBusinessLogic;
using IDataAccess;
using ModeloValidador.Abstracciones;
using PaginationAndFilters;
using PaginationAndFilters.Models;

namespace BusinessLogic;
public class CompanyService(ICompanyRepository companyRepository,
    IAssemblyLoadingService<IModeloValidador> assemblyLoadingService,
    ISessionService sessionService,
    ICompanyOwnerService companyOwnerService) : ICompanyService
{
    public Company Create(CreateCompanyArgs args, string sessionToken)
    {
        if (CompanyWithRutExists(args))
        {
            throw new InvalidOperationException("A company with the entered Rut has already been registered");
        }

        if (ModelValidatorDoesNotExist(args))
        {
            throw new KeyNotFoundException("Model validation implementation not found");
        }

        if (LoggedUserAlreadyHasACompanyRegistered(sessionToken))
        {
            throw new InvalidOperationException("User already has a company registered");
        }

        var newCompany = new Company
        {
            CompanyName = args.CompanyName,
            Logotype = args.Logotype,
            Rut = args.Rut,
            DeviceModelValidatorId = Guid.Parse(args.DeviceModelValidationId),
            CompanyOwner = companyOwnerService.GetById(sessionService.GetUserByToken(sessionToken).Id)
        };

        companyRepository.Add(newCompany);
        return newCompany;
    }

    public List<GetAllCompaniesCompanyArgs> GetAllCompanies(CompanyFilterArgs args)
    {
        if (PaginationFilterService.CurrentPageOrPageSizeIsNegative(args.Offset, args.Limit))
        {
            throw new FormatException("Neither page size nor current page can be negative.");
        }

        var companies = companyRepository.GetAll(args);

        var companiesToDTO = companies
            .ConvertAll(company =>
            new GetAllCompaniesCompanyArgs(
                company.CompanyName,
                company.CompanyOwner!.Name + " " + company.CompanyOwner.Surname,
                company.CompanyOwner.Email,
                company.Rut));

        return companiesToDTO;
    }

    public Company GetCompanyByOwnerId(Guid userId)
    {
        if (companyRepository.GetCompanyByOwnerId(userId) is null)
        {
            throw new KeyNotFoundException("Logged user does not have an associated company");
        }

        return companyRepository.GetCompanyByOwnerId(userId)!;
    }

    private bool CompanyWithRutExists(CreateCompanyArgs args)
    {
        return companyRepository.Exists(company => company.Rut == args.Rut);
    }

    private bool ModelValidatorDoesNotExist(CreateCompanyArgs args)
    {
        return assemblyLoadingService.GetImplementationById(Guid.Parse(args.DeviceModelValidationId)) is null;
    }

    private bool LoggedUserAlreadyHasACompanyRegistered(string sessionToken)
    {
        var loggedUser = sessionService.GetUserByToken(sessionToken);
        return companyOwnerService.GetById(loggedUser.Id).AssociatedCompany is not null;
    }
}
