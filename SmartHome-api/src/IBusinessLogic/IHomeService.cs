using Domain;
using DTOs;

namespace IBusinessLogic;

public interface IHomeService
{
    Home Create(CreateHomeArgs args);
    List<GetAllDevicesOfHomeArgs> ListHomeDevices(Guid homeId, string? room);
    Member AddMemberToHome(Guid homeId, AddMemberToHomeArgs memberArgs);
    HomeDevice AssociateDevice(Guid homeId, Guid deviceHardwareId);
    List<GetAllMembersOfHomeArgs> ListMembersOfHome(Guid homeId);
    void UpdateMemberNotifications(Guid homeId, UpdateMemberNotificationsArgs toArgs);
    Guid GetHomeIdByHardwareId(Guid cameraId);
    List<Member> GetHomeMembers(Guid homeId);
    Home AddRoomToHome(Guid homeId, string roomName);
    Room AddDeviceToRoomOfHome(Guid roomId, string hardwareId);
    Home AddAliasToHome(Guid homeId, string alias);
    Home GetHomeByHomeOwnerId(Guid homeOwnerId);
    List<GetHomesThatUserBelongsInArgs> GetHomesThatLoggedInUserBelongsTo(string token);
    List<GetAllRoomsOfHomeArgs> GetAllRoomsOfHome(Guid homeId);
}
