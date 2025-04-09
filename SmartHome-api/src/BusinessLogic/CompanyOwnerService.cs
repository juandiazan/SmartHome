using Domain;
using DTOs;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;
public sealed class CompanyOwnerService(ICompanyOwnerRepository companyOwnerRepository) : ICompanyOwnerService
{
    public CompanyOwner Create(CreateUserArgs args)
    {
        if (CompanyOwnerWithEmailExists(args))
        {
            throw new InvalidOperationException("A user with the entered email has already been registered");
        }

        var newCompanyOwner = new CompanyOwner
        {
            Name = args.Name,
            Surname = args.Surname,
            Email = args.Email,
            Password = args.Password,
            RoleId = companyOwnerRepository.GetCompanyOwnerRoleId(),
            Role = companyOwnerRepository.GetRoleById(companyOwnerRepository.GetCompanyOwnerRoleId())
        };

        companyOwnerRepository.Add(newCompanyOwner);

        return newCompanyOwner;
    }

    public CompanyOwner GetById(Guid userId)
    {
        if (CompanyOwnerDoesNotExist(userId))
        {
            throw new KeyNotFoundException("User does not exist");
        }

        return companyOwnerRepository.GetById(userId);
    }

    public CompanyOwner GiveHomeOwnerRoleToCompanyOwner(Guid userToBeUpdated)
    {
        var user = GetById(userToBeUpdated);

        user.RoleId = companyOwnerRepository.GetCompanyOwnerHomeOwnerRoleId();
        user.Role = companyOwnerRepository.GetRoleById(user.RoleId);

        companyOwnerRepository.Update(user);

        return user;
    }

    private bool CompanyOwnerDoesNotExist(Guid companyOwner)
    {
        return !companyOwnerRepository.Exists(co => co.Id == companyOwner);
    }

    private bool CompanyOwnerWithEmailExists(CreateUserArgs args)
    {
        return companyOwnerRepository.Exists(companyOwner => companyOwner.Email == args.Email);
    }
}
