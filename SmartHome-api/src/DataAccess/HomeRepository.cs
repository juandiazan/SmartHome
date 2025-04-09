using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using PaginationAndFilters;

namespace DataAccess;
public class HomeRepository(SmartHomeDBContext context) : IHomeRepository
{
    private readonly SmartHomeDBContext _context = context;
    private readonly DbSet<Home> _homes = context.Set<Home>();

    public Home Add(Home newHome)
    {
        _homes.Add(newHome);

        _context.SaveChanges();

        return newHome;
    }

    public HomeDevice AssociateDevice(Guid homeId, Guid hardwareId)
    {
        var home = _homes.FirstOrDefault(h => h.Id == homeId)!;
        var homeDevice = _context.Set<HomeDevice>().FirstOrDefault(hd => hd.HardwareId == hardwareId)!;

        var homeDeviceAsociated = home.AssociateDevice(homeDevice);

        _context.Update(home);
        _context.Update(homeDevice);

        _context.SaveChanges();
        return homeDeviceAsociated;
    }

    public Guid GetHomeIdByHardwareId(Guid hardwareId)
    {
        return _homes.ToList().FirstOrDefault(h => h.HasDevice(hardwareId))?.Id ?? Guid.Empty;
    }

    public bool IsAssociatedToAnyHome(Guid hardwareId)
    {
        return _homes.ToList().Any(c => c.HasDevice(hardwareId));
    }

    public Member AddMemberToHome(Guid homeToBeAddedToId, Member newMember)
    {
        _context.Members.Add(newMember);

        var home = _homes.FirstOrDefault(h => h.Id == homeToBeAddedToId)!;

        home.AddMember(newMember);

        _context.SaveChanges();

        return newMember;
    }

    public void UpdateMemberNotifications(Member memberToUpdate)
    {
        _context.Update(memberToUpdate);
        _context.SaveChanges();
    }

    public Home GetHomeById(Guid homeId)
    {
        return _context.Homes
            .Include(h => h.Rooms)
            .Include(h => h.Members).ThenInclude(m => m.Permissions)
            .FirstOrDefault(h => h.Id == homeId)!;
    }

    public List<HomeDevice> ListHomeDevices(Guid homeId, string? room)
    {
        var home =
            _context.Homes
            .Include(h => h.AssociatedDevices).ThenInclude(hd => hd.Device)
            .Include(h => h.AssociatedDevices).ThenInclude(hd => hd.RoomItIsIn)
            .FirstOrDefault(h => h.Id == homeId)!;

        return PaginationFilterService.FilterHomeDevices(home.AssociatedDevices, room);
    }

    public bool HomeExists(Guid homeId)
    {
        return _context.Homes.Any(h => h.Id == homeId);
    }

    public List<Member> ListMembersOfHome(Guid homeId)
    {
        var home = _context.Homes
            .Include(h => h.Members).ThenInclude(m => m.Permissions)
            .Include(m => m.Members).ThenInclude(m => m.AssociatedHomeOwner).FirstOrDefault(h => h.Id == homeId)!;

        return home.Members.ToList();
    }

    public List<Member> GetHomeMembers(Guid homeId)
    {
        var home = _context.Homes.Include(h => h.Members).ThenInclude(m => m.Permissions).FirstOrDefault(h => h.Id == homeId)!;

        return home.Members;
    }

    public User GetHomeOwnerByEmail(string argsOwnerEmail)
    {
        return _context.Users.FirstOrDefault(ho => ho.Email == argsOwnerEmail)!;
    }

    public User GetHomeOwnerByHomeId(Guid homeToBeAddedToId)
    {
        var user = _context.Homes.Include(h => h.HomeOwner).FirstOrDefault(h => h.Id == homeToBeAddedToId)!.HomeOwner;
        return user;
    }

    public Permission GetAddDevicesOfHomePermission()
    {
        return _context.Permissions.FirstOrDefault(p => p.Id == SmartHomeDBContext.AddDeviceToSpecificHomePermissionId)!;
    }

    public Permission GetListDevicesOfHomePermission()
    {
        return _context.Permissions.FirstOrDefault(p => p.Id == SmartHomeDBContext.ListDevicesOfSpecificHomePermissionId)!;
    }

    public Permission GetChangeAliasOfDevicesOfHomePermission()
    {
        return _context.Permissions.FirstOrDefault(p => p.Id == SmartHomeDBContext.ChangeHomeDeviceAliasPermissionId)!;
    }

    public Permission GetReceiveNotificationsPermission()
    {
        return _context.Permissions.FirstOrDefault(p => p.Id == SmartHomeDBContext.ReceiveNotificationsPermissionId)!;
    }

    public Member? GetMemberById(Guid memberId)
    {
        throw new NotImplementedException();
    }

    public Home UpdateHome(Home homeToBeUpdated, Room roomToBeAdded)
    {
        _context.Rooms.Add(roomToBeAdded);

        _context.Homes.Update(homeToBeUpdated);
        _context.SaveChanges();
        return homeToBeUpdated;
    }

    public Room? GetRoomById(Guid roomId)
    {
        return _context.Rooms.Include(r => r.HomeDevices).FirstOrDefault(r => r.Id == roomId);
    }

    public Room AddDeviceToRoom(HomeDevice homeDevice, Room room)
    {
        _context.Update(homeDevice);
        _context.Update(room);
        _context.SaveChanges();

        return room;
    }

    public Home UpdateHomeAlias(Home homeToBeUpdated)
    {
        _context.Homes.Update(homeToBeUpdated);
        _context.SaveChanges();
        return homeToBeUpdated;
    }

    public Home? GetHomeByHomeOwnerId(Guid homeOwnerId)
    {
        return _homes.Include(h => h.HomeOwner).FirstOrDefault(h => h.HomeOwner.Id == homeOwnerId);
    }

    public List<Home> GetHomesThatUserIsInById(Guid userId)
    {
        return [.. _homes.Include(h => h.HomeOwner).Include(h => h.Members).ThenInclude(m => m.Permissions)
            .Where(h =>
            h.HomeOwner.Id == userId ||
            h.Members.Any(m => m.AssociatedHomeOwnerId == userId))];
    }

    public List<Room> GetAllRoomsOfAHome(Guid homeId)
    {
        return _homes.Include(h => h.Rooms).FirstOrDefault(h => h.Id == homeId)!.Rooms;
    }
}
