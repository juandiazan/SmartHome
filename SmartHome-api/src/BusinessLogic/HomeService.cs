using System.Text.RegularExpressions;
using Domain;
using DTOs;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;
public class HomeService(IHomeRepository homeRepository, IHomeDeviceRepository homeDeviceRepository, ISessionService sessionService) : IHomeService
{
    public Home Create(CreateHomeArgs args)
    {
        var homeOwner = homeRepository.GetHomeOwnerByEmail(args.OwnerEmail);

        var newHome = new Home
        {
            OwnerEmail = args.OwnerEmail,
            HomeOwner = homeOwner,
            Address = new Address(args.MainStreet, args.DoorNumber),
            Location = new GeographicLocation(args.Latitude, args.Longitude),
            MaxAmountOfMembers = args.MaxAmountOfMembers,
            Alias = args.Alias
        };

        homeRepository.Add(newHome);

        var homeOwnerAsMember = new Member
        {
            Id = Guid.NewGuid(),
            AssociatedHomeOwner = homeOwner,
            Permissions =
            [
                homeRepository.GetAddDevicesOfHomePermission(),
                homeRepository.GetListDevicesOfHomePermission(),
                homeRepository.GetChangeAliasOfDevicesOfHomePermission(),
                homeRepository.GetReceiveNotificationsPermission()
            ]
        };
        homeRepository.AddMemberToHome(newHome.Id, homeOwnerAsMember);

        return newHome;
    }

    public HomeDevice AssociateDevice(Guid homeId, Guid hardwareId)
    {
        if (IsEmpty(hardwareId))
        {
            throw new ArgumentNullException(null, "Hardware Id cannot be empty");
        }

        if (GetHomeIdByHardwareId(hardwareId).ToString() == Guid.Empty.ToString())
        {
            throw new KeyNotFoundException("Device does not exist");
        }

        return homeRepository.AssociateDevice(homeId, hardwareId);
    }

    public Guid GetHomeIdByHardwareId(Guid hardwareId)
    {
        return homeRepository.GetHomeIdByHardwareId(hardwareId);
    }

    public List<Member> GetHomeMembers(Guid homeId)
    {
        if (HomeDoesNotExist(homeId))
        {
            throw new KeyNotFoundException("Home Id does not exist");
        }

        return homeRepository.GetHomeMembers(homeId);
    }

    public Member AddMemberToHome(Guid homeToBeAddedToId, AddMemberToHomeArgs newMemberToAddArgs)
    {
        if (!IsValidEmail(newMemberToAddArgs.EmailOfNewMember))
        {
            throw new ArgumentException("Invalid email format");
        }

        if (!homeRepository.HomeExists(homeToBeAddedToId))
        {
            throw new KeyNotFoundException("Home not found");
        }

        var homeOwnerToBeAdded = homeRepository.GetHomeOwnerByEmail(newMemberToAddArgs.EmailOfNewMember)
            ?? throw new InvalidOperationException("HomeOwner not found");

        var newMember = new Member
        {
            Id = Guid.NewGuid(),
            AssociatedHomeOwner = homeOwnerToBeAdded
        };

        if (newMemberToAddArgs.CanAddDeviceToHome)
        {
            newMember.Permissions.Add(homeRepository.GetAddDevicesOfHomePermission());
        }

        if (newMemberToAddArgs.CanSeeDevicesOfHome)
        {
            newMember.Permissions.Add(homeRepository.GetListDevicesOfHomePermission());
        }

        if (newMemberToAddArgs.CanChangeAliasOfDevices)
        {
            newMember.Permissions.Add(homeRepository.GetChangeAliasOfDevicesOfHomePermission());
        }

        var member = homeRepository.AddMemberToHome(homeToBeAddedToId, newMember);
        return member;
    }

    public Home GetHomeByHomeOwnerId(Guid homeOwnerId)
    {
        var home = homeRepository.GetHomeByHomeOwnerId(homeOwnerId);
        return home ?? throw new KeyNotFoundException("Home not found");
    }

    public List<GetAllDevicesOfHomeArgs> ListHomeDevices(Guid homeId, string? room)
    {
        if (homeId == Guid.Empty)
        {
            throw new FormatException("Home Id cannot be empty");
        }

        if (HomeDoesNotExist(homeId))
        {
            throw new FormatException("Home Id does not exist");
        }

        var homeDevices = homeRepository.ListHomeDevices(homeId, room);

        var returnList = new List<GetAllDevicesOfHomeArgs>();

        homeDevices.ForEach(hd =>
        {
            var roomItIsIn = hd.RoomItIsIn != null ? hd.RoomItIsIn.Name : "None";

            returnList.Add(new GetAllDevicesOfHomeArgs(
                hd.Device!.DeviceName,
                hd.Device.DeviceModel,
                hd.Device.Photos[0],
                hd.ConnectionState,
                hd.Alias,
                hd.HardwareId.ToString(),
                roomItIsIn));
        });

        return returnList;
    }

    public List<GetAllMembersOfHomeArgs> ListMembersOfHome(Guid homeId)
    {
        if (HomeDoesNotExist(homeId))
        {
            throw new KeyNotFoundException("Home Id does not exist");
        }

        var members = homeRepository.ListMembersOfHome(homeId);

        return members.ConvertAll(member => new GetAllMembersOfHomeArgs(
            member.Id.ToString(),
            member.AssociatedHomeOwner.Name + " " + member.AssociatedHomeOwner.Surname,
            member.AssociatedHomeOwner.Email,
            member.AssociatedHomeOwner is HomeOwner homeOwnerWithPicture ? homeOwnerWithPicture.ProfilePicture : "None",
            member.Permissions.ConvertAll(p => p.Name),
            member.Permissions.Contains(homeRepository.GetReceiveNotificationsPermission())));
    }

    public void UpdateMemberNotifications(Guid homeId, UpdateMemberNotificationsArgs args)
    {
        if (HomeDoesNotExist(homeId))
        {
            throw new KeyNotFoundException("Home Id does not exist");
        }

        var boolToSet = args.NotificationsEnabled;

        var member = homeRepository.GetHomeById(homeId).Members.FirstOrDefault(m => m.Id == args.MemberId);

        if (member is null)
        {
            throw new KeyNotFoundException("Member does not exist");
        }

        if (boolToSet)
        {
            if (!member.Permissions.Any(p => p.Name == "receive-notifications"))
            {
                member.Permissions.Add(homeRepository.GetReceiveNotificationsPermission());
            }
        }
        else
        {
            var permission = member!.Permissions.FirstOrDefault(p => p.Name == "receive-notifications");
            if (permission != null)
            {
                member.Permissions.Remove(permission);
            }
        }

        homeRepository.UpdateMemberNotifications(member);
    }

    public Home AddRoomToHome(Guid homeId, string roomName)
    {
        if (HomeDoesNotExist(homeId))
        {
            throw new KeyNotFoundException("Home does not exist");
        }

        if (string.IsNullOrEmpty(roomName))
        {
            throw new ArgumentNullException(null, "Room name cannot be null or empty");
        }

        var home = homeRepository.GetHomeById(homeId);

        if (HomeAlreadyHasRoomWithName(home, roomName))
        {
            throw new InvalidOperationException("Room with that name already exists in the home");
        }

        var newRoom = new Room
        {
            Name = roomName,
            HomeItBelongsToId = homeId
        };

        home.AddRoom(newRoom);

        var updatedHome = homeRepository.UpdateHome(home, newRoom);

        return updatedHome;
    }

    public Room AddDeviceToRoomOfHome(Guid roomId, string hardwareId)
    {
        if (HardwareIdFormatIsInvalid(hardwareId))
        {
            throw new ArgumentException("Home device format is invalid");
        }

        if (RoomDoesNotExist(roomId))
        {
            throw new KeyNotFoundException("Room does not exist");
        }

        if (HomeDeviceDoesNotExist(Guid.Parse(hardwareId)))
        {
            throw new KeyNotFoundException("Home device does not exist");
        }

        var room = homeRepository.GetRoomById(roomId)!;
        var homeDevice = homeDeviceRepository.GetHomeDeviceByHardwareId(Guid.Parse(hardwareId))!;

        if (HomeDeviceAlreadyIsInRoom(room, homeDevice))
        {
            throw new InvalidOperationException("Home device already is in room");
        }

        if (HomeDeviceDoesNotBelongToSameHomeAsRoom(room, homeDevice))
        {
            throw new InvalidOperationException("Home device does not belong to the room's home");
        }

        room.AddHomeDevice(homeDevice);
        homeDevice.RoomItIsInId = room.Id;

        homeRepository.AddDeviceToRoom(homeDevice, room);

        return room;
    }

    public Home AddAliasToHome(Guid homeId, string alias)
    {
        if (string.IsNullOrEmpty(alias))
        {
            throw new ArgumentNullException(null, "Alias cannot be null or empty");
        }

        if (HomeDoesNotExist(homeId))
        {
            throw new KeyNotFoundException("Home does not exist");
        }

        var home = homeRepository.GetHomeById(homeId);
        home.Alias = alias;

        var updatedHome = homeRepository.UpdateHomeAlias(home);

        return updatedHome;
    }

    public List<GetHomesThatUserBelongsInArgs> GetHomesThatLoggedInUserBelongsTo(string token)
    {
        var user = sessionService.GetUserByToken(token);

        var homes = homeRepository.GetHomesThatUserIsInById(user.Id);

        var homesToDto = new List<GetHomesThatUserBelongsInArgs>();

        homes.ForEach(home =>
        {
            if (home.OwnerEmail.Equals(user.Email))
            {
                homesToDto.Add(
                    new GetHomesThatUserBelongsInArgs(
                        home.Id.ToString(),
                        home.Alias,
                        ["list-devices-of-specific-home", "add-device-to-specific-home", "change-alias-of-specific-device", "receive-notifications"],
                        true));
            }
            else
            {
                homesToDto.Add(
                    new GetHomesThatUserBelongsInArgs(
                        home.Id.ToString(),
                        home.Alias,
                        home.Members.First(m => m.AssociatedHomeOwnerId == user.Id).Permissions.ConvertAll(p => p.Name),
                        false));
            }
        });

        return homesToDto;
    }

    public List<GetAllRoomsOfHomeArgs> GetAllRoomsOfHome(Guid homeId)
    {
        if (HomeDoesNotExist(homeId))
        {
            throw new KeyNotFoundException("Home does not exist");
        }

        var rooms = homeRepository.GetAllRoomsOfAHome(homeId);

        return rooms.ConvertAll(r => new GetAllRoomsOfHomeArgs(r.Id.ToString(), r.Name));
    }

    private static bool HomeDeviceAlreadyIsInRoom(Room room, HomeDevice homeDevice)
    {
        return room.HomeDevices.Any(hd => hd.HardwareId == homeDevice.HardwareId);
    }

    private static bool HardwareIdFormatIsInvalid(string hardwareId)
    {
        return !Guid.TryParse(hardwareId, out _);
    }

    private static bool HomeDeviceDoesNotBelongToSameHomeAsRoom(Room room, HomeDevice homeDevice)
    {
        return room.HomeItBelongsToId != homeDevice.HomeId;
    }

    private bool RoomDoesNotExist(Guid roomId)
    {
        return homeRepository.GetRoomById(roomId) is null;
    }

    private bool HomeDeviceDoesNotExist(Guid hardwareId)
    {
        return homeDeviceRepository.GetHomeDeviceByHardwareId(hardwareId) is null;
    }

    private static bool HomeAlreadyHasRoomWithName(Home home, string roomName)
    {
        return home.Rooms.Any(r => r.Name == roomName);
    }

    private static bool IsEmpty(Guid hardwareId)
    {
        return hardwareId.ToString() == Guid.Empty.ToString();
    }

    private static bool IsValidEmail(string email)
    {
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return emailRegex.IsMatch(email);
    }

    private bool HomeDoesNotExist(Guid homeId)
    {
        return !homeRepository.HomeExists(homeId);
    }
}
