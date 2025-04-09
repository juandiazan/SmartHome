using System.Linq.Expressions;
using Domain;
using DTOs;
using IBusinessLogic;
using IDataAccess;
using PaginationAndFilters;
using PaginationAndFilters.Models;

namespace BusinessLogic;

public class UserService(IAdministratorRepository userRepository, IUserHasSessionActive sessionService) : IUserService
{
    public User Create(CreateUserArgs args)
    {
        if (userRepository.Exists(u => u.Email == args.Email))
        {
            throw new InvalidOperationException("A user with the entered email has already been registered");
        }

        var newUser = new User
        {
            Name = args.Name,
            Surname = args.Surname,
            Email = args.Email,
            Password = args.Password,
            RoleId = userRepository.GetAdminRoleId(),
            Role = userRepository.GetRoleById(userRepository.GetAdminRoleId())
        };

        userRepository.Add(newUser);

        return newUser;
    }

    public List<GetAllUserArgs> GetAll(UserFilterArgs args)
    {
        if (PaginationFilterService.CurrentPageOrPageSizeIsNegative(args.Offset, args.Limit))
        {
            throw new FormatException("Current page and page size cannot be negative or zero");
        }

        var users = userRepository.GetAll(args);

        return users.ConvertAll(user =>
        new GetAllUserArgs(
            user.Id.ToString(),
            user.Name,
            user.Surname,
            user.Name + " " + user.Surname,
            user.Role.RoleName,
            user.CreationDate.ToString()));
    }

    public User DeleteById(Guid userId)
    {
        if (UserDoesNotExist(userId))
        {
            throw new KeyNotFoundException("User to delete does not exist");
        }

        if (sessionService.HasActiveSessionById(userId))
        {
            throw new InvalidOperationException("User cannot be deleted because of an active session");
        }

        return userRepository.DeleteById(userId);
    }

    public bool Exists(Expression<Func<User, bool>> predicate)
    {
        return userRepository.Exists(predicate);
    }

    public User GetUserById(Guid userId)
    {
        if (UserDoesNotExist(userId))
        {
            throw new KeyNotFoundException("User does not exist");
        }

        return userRepository.GetUserById(userId);
    }

    public User GetUserByEmail(string email)
    {
        if (!userRepository.Exists(u => u.Email == email))
        {
            throw new KeyNotFoundException("User with entered email does not exist");
        }

        return userRepository.GetUserByEmail(email);
    }

    public bool IsPasswordCorrect(string email, string password)
    {
        return userRepository.IsPasswordCorrect(email, password);
    }

    public User UpdateRoleOfAdministratorToHomeOwner(Guid userToBeUpdated)
    {
        var user = GetUserById(userToBeUpdated);

        user.RoleId = userRepository.GetAdminHomeOwnerRoleId();
        user.Role = userRepository.GetRoleById(user.RoleId);

        userRepository.Update(user);

        return user;
    }

    private bool UserDoesNotExist(Guid userId)
    {
        return !userRepository.Exists(u => u.Id == userId);
    }
}
