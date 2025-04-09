using Domain;

namespace IDataAccess;
public interface IHomeRepository : IAddRepository<Home>
{
    HomeDevice AssociateDevice(Guid homeId, Guid hardwareId);
    Guid GetHomeIdByHardwareId(Guid hardwareId);
    Member AddMemberToHome(Guid homeToBeAddedToId, Member newMember);

    List<HomeDevice> ListHomeDevices(Guid homeId, string? room);
    bool HomeExists(Guid homeId);
    List<Member> ListMembersOfHome(Guid homeId);
    List<Member> GetHomeMembers(Guid homeId);
    void UpdateMemberNotifications(Member memberToUpdate);
    User GetHomeOwnerByHomeId(Guid homeToBeAddedToId);
    User GetHomeOwnerByEmail(string argsOwnerEmail);
    Permission GetAddDevicesOfHomePermission();
    Permission GetListDevicesOfHomePermission();
    Permission GetChangeAliasOfDevicesOfHomePermission();

    Permission GetReceiveNotificationsPermission();

    Home GetHomeById(Guid homeId);
    Home UpdateHome(Home homeToBeUpdated, Room roomToBeAdded);
    Room? GetRoomById(Guid roomId);
    Room AddDeviceToRoom(HomeDevice homeDevice, Room room);
    Home UpdateHomeAlias(Home homeToBeUpdated);
    Home? GetHomeByHomeOwnerId(Guid homeOwnerId);

    List<Home> GetHomesThatUserIsInById(Guid userId);
    List<Room> GetAllRoomsOfAHome(Guid homeId);
}
