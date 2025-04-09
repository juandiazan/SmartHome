using Domain;
using DTOs;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;
public sealed class HomeOwnerService : IHomeOwnerService
{
    private readonly IHomeOwnerRepository _homeOwnerRepository;
    private readonly ISessionService _sessionService;
    private readonly IHomeService _homeService;

    public HomeOwnerService(IHomeOwnerRepository homeOwnerRepository, ISessionService sessionService, IHomeService homeService)
    {
        _homeOwnerRepository = homeOwnerRepository;
        _sessionService = sessionService;
        _homeService = homeService;
    }

    public HomeOwner Create(CreateHomeOwnerArgs args)
    {
        if (HomeOwnerWithEmailExists(args))
        {
            throw new InvalidOperationException("A user with the entered email has already been registered");
        }

        var newHomeOwner = new HomeOwner
        {
            Name = args.Name,
            Surname = args.Surname,
            ProfilePicture = args.ProfilePicture,
            Email = args.Email,
            Password = args.Password,
            CreationDate = DateTime.Now,
            RoleId = _homeOwnerRepository.GetHomeOwnerRoleId(),
            Role = _homeOwnerRepository.GetRoleById(_homeOwnerRepository.GetHomeOwnerRoleId())
        };

        _homeOwnerRepository.Add(newHomeOwner);

        return newHomeOwner;
    }

    public string GetHomeOwnerOwnedHomeId(string token)
    {
        var homeOwner = _sessionService.GetUserByToken(token);
        var home = _homeService.GetHomeByHomeOwnerId(homeOwner.Id);
        return home.Id.ToString();
    }

    private bool HomeOwnerWithEmailExists(CreateHomeOwnerArgs args)
    {
        return _homeOwnerRepository.Exists(ho => ho.Email == args.Email);
    }
}
